using API.Services;
using Filuet.Infrastructure.Abstractions.Converters;
using Filuet.Infrastructure.Communication.Helpers;
using Filuet.Infrastructure.DataProvider.Interfaces;
using Filuet.Infrastructure.DataProvider;
using MessagingServices;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.Kiosks.Services;
using MPT.Vending.Domains.Ordering.Abstractions;
using MPT.Vending.Domains.Ordering.Services;
using MPT.Vending.Domains.SharedContext.Abstractions;
using MPT.Vending.Domains.SharedContext.Services;
using MPT.Vending.Domains.Advertisement.Services;
using MPT.Vending.Domains.Advertisement.Abstractions;
using System.Text;
using System.Text.Json;
using Filuet.Hardware.Dispensers.Abstractions.Helpers;

var builder = WebApplication.CreateBuilder(args);
IKioskService _mediatorKioskService = null;
IReplenishmentService _mediatorReplenishmentService = null;
IProductService _mediatorProductService = null;
IMediaService _mediatorMediaService = null;
StockBalance stockBalance = null;

// bind common json converters
builder.Services.AddControllers()
    .AddJsonOptions(opts => {
        opts.JsonSerializerOptions.Converters.Add(new CurrencyJsonConverter());
        opts.JsonSerializerOptions.Converters.Add(new CountryJsonConverter());
        opts.JsonSerializerOptions.Converters.Add(new LanguageJsonConverter());
        opts.JsonSerializerOptions.Converters.Add(new N2JsonConverter());
    });

var p2kMediator = new Portal2KioskMessagesSender(AzureKeyVaultReader.GetSecret("ogmento-servicebus"));

// if Mode is 'Demo' then the api won't establish the DB connection and will use inmemory data storage
// use 'Demo' only for testing and demonstration purposes
string mode = builder.Configuration["Mode"];

string connectionString = string.Empty;

if (!string.Equals(mode, "demo", StringComparison.InvariantCultureIgnoreCase)) {
    connectionString = AzureKeyVaultReader.GetSecret("dbcs-ogmento-dev"); // get the db connection string if not in the demo mode
}

builder.Services.AddKiosk(x => {
    x.onKioskChanged += async (sender, e) => await p2kMediator.OnKioskHasChanged(sender, e); // notify ui of changes

    x.onPlanogramChanged += async (sender, e) => {
        // notify portal about planogram changes
        int index = 0;
        while (true) {
            string? portalUrl = builder.Configuration[$"Portal:{index++}"];
            if (!string.IsNullOrWhiteSpace(portalUrl)) {
                HttpClient client = new HttpClient();
                var httpContent = new StringContent(JsonSerializer.Serialize(new TransactionHookRequest {
                    Message = HookHelpers.Encrypt(AzureKeyVaultReader.GetSecret("ogmentoportal-hook-secret"),
                    JsonSerializer.Serialize(new PlanogramHook { KioskUid = e.KioskUid, Planogram = e.Planogram }))
                }), Encoding.UTF8, "application/json");

                await client.PostAsync(new Uri(new Uri(portalUrl), "/api/hook/planogram"), httpContent);
            }
            else break;
        }

        // update stock keeper
        var runningLowProducts = e.Planogram.GetStock();
        stockBalance.Update(e.KioskUid, runningLowProducts.Select(x => new ProductStock {
            ProductUid = x.productUid,
            Quantuty = x.count,
            MaxQuantuty = x.maxCount
        }));
    };
},
    x => x.onPlanogramChanged += async (sender, e) => {
        /* to be done  await mediator.OnPlanogramHasChanged(sender, e) */
    },
    x => _mediatorProductService.GetAsync(x.Distinct()).ToBlockingEnumerable().ToList(),
    x => _mediatorMediaService.GetByKiosks(x),
    connectionString);

builder.Services.AddOrdering(x => x.onProductChanged += async (sender, e) =>
    await p2kMediator.OnProductHasChanged(sender, e, _mediatorKioskService.GetKiosksWithSku(e.Sku)), connectionString);

builder.Services.AddTransient<IBlobRepository>(sp => new AzureBlobRepository(x => {
    x.ConnectionString = AzureKeyVaultReader.GetSecret("azurestoragecs-dev");
    x.ContainerName = "ogmento";
}))
.AddAdvertisement(connectionString);

builder.Services.AddSingleton(sp => new StockBalance(() => sp.GetService<IReplenishmentService>().GetPlanograms()
    .Select(x => new KioskStock {
        KioskUid = x.Key,
        Balance = x.Value.GetStock().Select(p => new ProductStock {
            ProductUid = p.productUid,
            Quantuty = p.count,
            MaxQuantuty = p.maxCount
        })
    }))
);

builder.Services.AddSingleton<IMemoryCachingService, MemoryCachingService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();

var app = builder.Build();

_mediatorKioskService = app.Services.GetService<IKioskService>();
_mediatorReplenishmentService = app.Services.GetService<IReplenishmentService>();
_mediatorProductService = app.Services.GetService<IProductService>();
_mediatorMediaService = app.Services.GetService<IMediaService>();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.MapRazorPages();

app.Run();