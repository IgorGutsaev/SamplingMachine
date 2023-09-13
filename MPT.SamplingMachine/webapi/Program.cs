using Filuet.Infrastructure.DataProvider;
using Filuet.Infrastructure.DataProvider.Interfaces;
using MPT.SamplingMachine.ApiClient;
using webapi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMemoryCachingService, MemoryCachingService>();
builder.Services.AddSingleton(sp => new SamplingMachineApiClient("https://localhost:7189/"));
builder.Services.AddSingleton(sp => new KioskService(sp.GetRequiredService<SamplingMachineApiClient>(), sp.GetRequiredService<IMemoryCachingService>(), "foo"));

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
