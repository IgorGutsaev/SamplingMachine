using Filuet.Infrastructure.DataProvider;
using Filuet.Infrastructure.DataProvider.Interfaces;
using MPT.SamplingMachine.ApiClient;
using webapi.Communication;
using webapi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

string kioskUid = "foo";

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMemoryCachingService, MemoryCachingService>();
builder.Services.AddSingleton(sp => new SamplingMachineApiClient("https://localhost:7189/"));
builder.Services.AddSingleton(sp => new KioskService(sp.GetRequiredService<SamplingMachineApiClient>(), sp.GetRequiredService<IMemoryCachingService>(), kioskUid));
builder.Services.AddSingleton(new Portal2KioskMessagesReceiver("Endpoint=sb://ogmento.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=26jG7d1B6ekEe+V7yd2OpVwEH+YauCLz1+ASbKg3R54=", kioskUid));

var app = builder.Build();

var processor = app.Services.GetRequiredService<Portal2KioskMessagesReceiver>();
await processor.RunAsync();

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
