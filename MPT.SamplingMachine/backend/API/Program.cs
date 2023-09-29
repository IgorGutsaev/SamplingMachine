using Filuet.Infrastructure.Abstractions.Converters;
using Filuet.Infrastructure.Communication.Helpers;
using MessagingServices;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.Kiosks.Services;
using MPT.Vending.Domains.Products.Abstractions;
using MPT.Vending.Domains.Products.Services;
using System.Text.Json;
using System.Text;

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
builder.Services.AddTransient<IProductService, DemoProductService>();
builder.Services.AddTransient<IKioskService>(sp => {
    DemoKioskService result = new DemoKioskService();
    result.onKioskHasChanged += async (sender, e) => await sp.GetRequiredService<Portal2KioskMessagesSender>().OnKioskHasChanged(sender, e);
    return result;
});
builder.Services.AddTransient<ISessionService>(sp => {
    DemoSessionService result = new DemoSessionService();
    result.OnNewSession += async (sender, e) => {
        IConfiguration config = sp.GetRequiredService<IConfiguration>();
        int index = 0;
        while (true)
        {
            string apiUrl = config[$"Api:{index++}"];
            if (!string.IsNullOrWhiteSpace(apiUrl))
            {
                HttpClient client = new HttpClient();
                var httpContent = new StringContent(JsonSerializer.Serialize(new SessionHookRequest { Message = HookHelpers.Encrypt(config["HookSecret"], JsonSerializer.Serialize(e)) }), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(new Uri(new Uri(apiUrl), "/api/hook/session"), httpContent);
            }
            else break;
        }
    };
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