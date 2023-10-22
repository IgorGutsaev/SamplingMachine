using Filuet.Infrastructure.Abstractions.Converters;
using Filuet.Infrastructure.Communication.Helpers;
using MessagingServices;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Advertisement.Abstractions;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.Kiosks.Services;
using MPT.Vending.Domains.Products.Abstractions;
using MPT.Vending.Domains.Products.Services;
using System.Text.Json;
using System.Text;
using MPT.Vending.Domains.Advertisement.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
     .AddJsonOptions(opts =>
     {
         opts.JsonSerializerOptions.Converters.Add(new CurrencyJsonConverter());
         opts.JsonSerializerOptions.Converters.Add(new CountryJsonConverter());
         opts.JsonSerializerOptions.Converters.Add(new LanguageJsonConverter());
         opts.JsonSerializerOptions.Converters.Add(new N2JsonConverter());
     });

builder.Services.AddSingleton(new Portal2KioskMessagesSender("Endpoint=sb://ogmento.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=26jG7d1B6ekEe+V7yd2OpVwEH+YauCLz1+ASbKg3R54="));
builder.Services.AddTransient<IKioskService>(sp => {
    DemoKioskService result = new DemoKioskService();
    result.onKioskChanged += async (sender, e) => await sp.GetRequiredService<Portal2KioskMessagesSender>().OnKioskHasChanged(sender, e);
    return result;
});

builder.Services.AddTransient<IProductService>(sp => {
    DemoProductService result = new DemoProductService();
    IKioskService kioskService = sp.GetService<IKioskService>();
    result.onProductChanged += async (sender, e) => await sp.GetRequiredService<Portal2KioskMessagesSender>().OnProductHasChanged(sender, e, kioskService.Get(x => x.ProductLinks.Any(l => l.Product.Sku == e.Sku)));
    return result;
});

builder.Services.AddTransient<ISessionService>(sp => {
    DemoSessionService result = new DemoSessionService();
    result.OnNewSession += async (sender, e) => {
        IConfiguration config = sp.GetRequiredService<IConfiguration>();
        int index = 0;
        while (true)
        {
            string portalUrl = config[$"Portal:{index++}"];
            if (!string.IsNullOrWhiteSpace(portalUrl))
            {
                HttpClient client = new HttpClient();
                var httpContent = new StringContent(JsonSerializer.Serialize(new SessionHookRequest { Message = HookHelpers.Encrypt(config["HookSecret"], JsonSerializer.Serialize(e)) }), Encoding.UTF8, "application/json");
                try
                {
                    HttpResponseMessage response = await client.PostAsync(new Uri(new Uri(portalUrl), "/api/hook/session"), httpContent);
                }
                catch { }
            }
            else break;
        }
    };
    return result;
});

builder.Services.AddTransient<IBlobRepository>(sp => new AzureBlobRepository(x => {
    x.ConnectionString = "DefaultEndpointsProtocol=https;AccountName=ascdevstorage;AccountKey=X5lm0IwRvY7gzf7EChalkTLTwCWk5croT7MESc44MkCY3y3EKXLfL9IRd1wSdUH5tyGcsWH7vUIrD5vXydcsEg==;EndpointSuffix=core.windows.net";
    x.ContainerName = "ogmento";
}))
.AddTransient<IMediaService, DemoMediaService>();

builder.Services.AddTransient<IReplenishmentService>(sp => {
    DemoReplenishmentService result = new DemoReplenishmentService();
    //result.onPlanogramChanged += async (sender, e) => await sp.GetRequiredService<Portal2KioskMessagesSender>().OnPlanogramHasChanged(sender, e));
    return result;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();