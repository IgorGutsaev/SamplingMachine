using Filuet.Infrastructure.Abstractions.Converters;
using Filuet.Infrastructure.DataProvider.Interfaces;
using Filuet.Infrastructure.DataProvider;
using MessagingServices;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.Kiosks.Services;
using MPT.Vending.Domains.Ordering.Abstractions;
using MPT.Vending.Domains.Ordering.Services;
using MPT.Vending.Domains.SharedContext.Abstractions;
using MPT.Vending.Domains.SharedContext.Services;
using MPT.Vending.Domains.Advertisement.Services;
using MPT.Vending.Domains.Advertisement.Abstractions;
using System.Text.Json;
using MPT.Vending.API.Dto;
using Filuet.Infrastructure.Communication.Helpers;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
IKioskService _mediatorKioskService = null;
IProductService _mediatorProductService = null;
IMediaService _mediatorMediaService = null;

builder.Services.AddControllers()
    .AddJsonOptions(opts => {
        opts.JsonSerializerOptions.Converters.Add(new CurrencyJsonConverter());
        opts.JsonSerializerOptions.Converters.Add(new CountryJsonConverter());
        opts.JsonSerializerOptions.Converters.Add(new LanguageJsonConverter());
        opts.JsonSerializerOptions.Converters.Add(new N2JsonConverter());
    });

Portal2KioskMessagesSender mediator = new Portal2KioskMessagesSender("Endpoint=sb://ogmento.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=26jG7d1B6ekEe+V7yd2OpVwEH+YauCLz1+ASbKg3R54=");

string mode = builder.Configuration["Mode"];

string connectionString = string.Empty;
if (!string.Equals(mode, "demo", StringComparison.InvariantCultureIgnoreCase)) {
    connectionString = "Data Source=tcp:ascmwsql.database.windows.net,1433;Initial Catalog=ogmento;Persist Security Info=True;User ID=filuetadmin;Password=Filuet@123!;MultipleActiveResultSets=False;Connect Timeout=45;Encrypt=True;TrustServerCertificate=False;Column Encryption Setting=Enabled";
}

builder.Services.AddKiosk(x => {
        x.onKioskChanged += async (sender, e) => await mediator.OnKioskHasChanged(sender, e); // notify ui of changes
        
        x.onPlanogramChanged += async (sender, e) => { // notify portal about planogram changes
            int index = 0;
            while (true) {
                string? portalUrl = builder.Configuration[$"Portal:{index++}"];
                if (!string.IsNullOrWhiteSpace(portalUrl)) {
                    HttpClient client = new HttpClient();
                    var httpContent = new StringContent(JsonSerializer.Serialize(new TransactionHookRequest {
                        Message = HookHelpers.Encrypt(builder.Configuration["HookSecret"],
                        JsonSerializer.Serialize(new PlanogramHook { KioskUid = e.KioskUid, Planogram = e.Planogram }))
                    }), Encoding.UTF8, "application/json");

                    await client.PostAsync(new Uri(new Uri(portalUrl), "/api/hook/planogram"), httpContent);
                }
                else break;
            }
        };
    },
    x => x.onPlanogramChanged += async (sender, e) => {
        /* to be done  await mediator.OnPlanogramHasChanged(sender, e) */
    },
    x => _mediatorProductService.GetAsync(x.Distinct()).ToBlockingEnumerable().ToList(),
    x => _mediatorMediaService.GetByKiosks(x),
    connectionString);

builder.Services.AddOrdering(x => x.onProductChanged += async (sender, e) =>
    await mediator.OnProductHasChanged(sender, e, _mediatorKioskService.GetKiosksWithSku(e.Sku)), connectionString);

builder.Services.AddTransient<IBlobRepository>(sp => new AzureBlobRepository(x => {
    x.ConnectionString = "DefaultEndpointsProtocol=https;AccountName=ascdevstorage;AccountKey=X5lm0IwRvY7gzf7EChalkTLTwCWk5croT7MESc44MkCY3y3EKXLfL9IRd1wSdUH5tyGcsWH7vUIrD5vXydcsEg==;EndpointSuffix=core.windows.net";
    x.ContainerName = "ogmento";
}))
.AddAdvertisement(connectionString);

builder.Services.AddSingleton<IMemoryCachingService, MemoryCachingService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

_mediatorKioskService = app.Services.GetService<IKioskService>();
_mediatorProductService = app.Services.GetService<IProductService>();
_mediatorMediaService = app.Services.GetService<IMediaService>();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();