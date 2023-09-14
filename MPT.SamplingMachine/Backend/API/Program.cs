using Filuet.Infrastructure.Abstractions.Converters;
using MPT.Vending.Domains.Products.Abstractions;
using MPT.Vending.Domains.Products.Services;

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

builder.Services.AddTransient<IProductService, ProductService>();

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