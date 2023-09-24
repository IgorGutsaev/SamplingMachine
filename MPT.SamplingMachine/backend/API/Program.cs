using Filuet.Infrastructure.Abstractions.Converters;
using MessagingServices;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.Kiosks.Services;
using MPT.Vending.Domains.Products.Abstractions;
using MPT.Vending.Domains.Products.Services;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
     .AddJsonOptions(opts =>
     {
         opts.JsonSerializerOptions.Converters.Add(new CurrencyJsonConverter());
         opts.JsonSerializerOptions.Converters.Add(new CountryJsonConverter());
         opts.JsonSerializerOptions.Converters.Add(new LanguageJsonConverter());
         opts.JsonSerializerOptions.Converters.Add(new N2JsonConverter());
     });

builder.Services.AddSingleton(new Portal2KioskMessagesSender("Endpoint=sb://ogmento.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=26jG7d1B6ekEe+V7yd2OpVwEH+YauCLz1+ASbKg3R54="));
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IKioskService>(sp => {
    DemoKioskService result = new DemoKioskService();
    result.onKioskHasChanged += async (sender, e) => await sp.GetRequiredService<Portal2KioskMessagesSender>().OnKioskHasChanged(sender, e);
    return result;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();