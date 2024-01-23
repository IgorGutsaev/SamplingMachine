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
using MPT.Vending.Domains.Identity.Services;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using API.Swagger;
using MPT.Vending.Domains.SharedContext;
using MPT.Vending.Domains.Identity.Abstractions;
using Filuet.Hardware.Dispensers.Abstractions.Helpers;
using MPT.Vending.Domains.Kiosks.Abstractions.Events;

var builder = WebApplication.CreateBuilder(args);
IKioskService _mediatorKioskService = null;
IReplenishmentService _mediatorReplenishmentService = null;
IProductService _mediatorProductService = null;
IMediaService _mediatorMediaService = null;
StockCache _stockBalance = null;

// bind common json converters
builder.Services.AddControllers(options =>
    options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build())))
.AddJsonOptions(opts => {
    opts.JsonSerializerOptions.Converters.Add(new CurrencyJsonConverter());
    opts.JsonSerializerOptions.Converters.Add(new CountryJsonConverter());
    opts.JsonSerializerOptions.Converters.Add(new LanguageJsonConverter());
    opts.JsonSerializerOptions.Converters.Add(new N2JsonConverter());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => {
        x.TokenValidationParameters = new TokenValidationParameters {
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AzureKeyVaultReader.GetSecret("ogmento-jwt-key"))),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization(options => {
    options.AddPolicy(IdentityData.AdminUserPolicyName, p => p.RequireClaim(IdentityData.AdminUserClaimName, "true"));
    options.AddPolicy(IdentityData.KioskUserPolicyName, p => p.RequireClaim(IdentityData.KioskUserPolicyName));
    options.AddPolicy(IdentityData.ManagerPolicyName, p => p.RequireAssertion(context =>
        context.User.HasClaim(c => c.Type == IdentityData.AdminUserClaimName || c.Type == IdentityData.ManagerClaimName))); // AdminUser is a manager as well
});

var p2kMediator = new Portal2KioskMessagesSender(AzureKeyVaultReader.GetSecret("ogmento-servicebus"));

// if Mode is 'Demo' then the api won't establish the DB connection and will use inmemory data storage
// use 'Demo' only for testing and demonstration purposes
string mode = builder.Configuration["Mode"];

string connectionString = string.Empty;

if (!string.Equals(mode, "demo", StringComparison.InvariantCultureIgnoreCase)) {
    connectionString = AzureKeyVaultReader.GetSecret("dbcs-ogmento-dev"); // get the db connection string if not in the demo mode

    SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder(connectionString);
    connStringBuilder.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled; // Enable Always Encrypted for the connection
    connectionString = connStringBuilder.ConnectionString; // This is the only change specific to Always Encrypted

    SqlColumnEncryptionAzureKeyVaultProvider azureKeyVaultProvider = new SqlColumnEncryptionAzureKeyVaultProvider(AzureKeyVaultReader._credential);
    Dictionary<string, SqlColumnEncryptionKeyStoreProvider> providers = new Dictionary<string, SqlColumnEncryptionKeyStoreProvider>();
    providers.Add(SqlColumnEncryptionAzureKeyVaultProvider.ProviderName, azureKeyVaultProvider);
    SqlConnection.RegisterColumnEncryptionKeyStoreProviders(providers);
}

// update stock cache
Action<PlanogramChangeEventArgs> whenPlanogramChanged = e => {
    var planograms = _mediatorReplenishmentService.GetPlanograms();
    IEnumerable<Product> products = _mediatorProductService.GetScopeAsync(planograms.Values.SelectMany(x => x.Products.Select(p => p.ProductUid)).Distinct()).Result;

    _stockBalance?.Update(e.KioskUid, e.Planogram.GetStock().Select(x => new ProductStock {
        Uid = x.productUid,
        Name = products.FirstOrDefault(k => k.Sku == x.productUid)?.Names?.FirstOrDefault()?.Value ?? x.productUid,
        Quantity = x.count,
        MaxQuantuty = x.maxCount
    }));
};

builder.Services.AddLocalIdentity(connectionString);
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
                    JsonSerializer.Serialize(new PlanogramHook { KioskUid = e.KioskUid.ToUpper(), Planogram = e.Planogram }))
                }), Encoding.UTF8, "application/json");

                await client.PostAsync(new Uri(new Uri(portalUrl), "/api/hook/planogram"), httpContent);
            }
            else break;
        }

        whenPlanogramChanged(e);
    };
},
    x => x.onPlanogramChanged += async (sender, e) => {
        /* to be done  await mediator.OnPlanogramHasChanged(sender, e) */
        whenPlanogramChanged(e);
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

builder.Services.AddSingleton( sp => {
    IProductService productService = sp.GetRequiredService<IProductService>();
    var planograms = sp.GetService<IReplenishmentService>().GetPlanograms();
    IEnumerable<Product> products = productService.GetScopeAsync(planograms.Values.SelectMany(x => x.Products.Select(p => p.ProductUid)).Distinct()).Result;

    return new StockCache(() => planograms.Select(x => new KioskStock {
        KioskUid = x.Key,
        Stock = x.Value.GetStock().Select(p => new ProductStock {
            Uid = p.productUid,
            Name = products.FirstOrDefault(k => k.Sku == p.productUid)?.Names?.FirstOrDefault()?.Value ?? p.productUid,
            Quantity = p.count,
            MaxQuantuty = p.maxCount
        })
    }));
});

builder.Services.AddSingleton<IMemoryCachingService, MemoryCachingService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddRazorPages();

var app = builder.Build();

_mediatorKioskService = app.Services.GetService<IKioskService>();
_mediatorReplenishmentService = app.Services.GetService<IReplenishmentService>();
_mediatorProductService = app.Services.GetService<IProductService>();
_mediatorMediaService = app.Services.GetService<IMediaService>();
_stockBalance = app.Services.GetService<StockCache>();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.MapRazorPages();

app.Run();