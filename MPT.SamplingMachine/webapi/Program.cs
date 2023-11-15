using CondomatProtocol;
using Filuet.Infrastructure.DataProvider;
using Filuet.Infrastructure.DataProvider.Interfaces;
using Microsoft.AspNetCore.SignalR;
using MPT.SamplingMachine.ApiClient;
using System.IO.Ports;
using webapi.Communication;
using webapi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMemoryCachingService, MemoryCachingService>();
builder.Services.AddSingleton(sp => new SamplingMachineApiClient(sp.GetService<IConfiguration>()["ApiUrl"]));
builder.Services.AddSingleton(sp => {
    string kioskUid = sp.GetService<IConfiguration>()["KioskUid"];
    return new KioskService(sp.GetRequiredService<SamplingMachineApiClient>(), sp.GetRequiredService<IMemoryCachingService>(), kioskUid);
});

//builder.Services.AddScoped<NotificationHub>();
builder.Services.AddSingleton(sp => {
    string kioskUid = sp.GetService<IConfiguration>()["KioskUid"];
    return new Portal2KioskMessagesReceiver("Endpoint=sb://ogmento.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=26jG7d1B6ekEe+V7yd2OpVwEH+YauCLz1+ASbKg3R54=", kioskUid, sp.GetRequiredService<IHubContext<NotificationHub>>());
});

builder.Services.AddSingleton(sp => {
    int portId = Convert.ToInt32(sp.GetService<IConfiguration>()["SerialPort"]);
    
    CondomatCommunicationService vmcService = new CondomatCommunicationService(portId);

    vmcService.onEvent += (sender, e) => {
        Console.WriteLine($"{DateTime.Now:T}: {(e.IsCommand ? ">" : "<")} {e.ResponseToString}" + (!string.IsNullOrWhiteSpace(e.Comment) ? $" ({e.Comment})" : string.Empty));
    };

    return vmcService;
});

builder.Services.AddCors(options => {
    options.AddPolicy("CorsPolicy",
        builder => {
            builder.WithOrigins("https://localhost:5002/")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});
builder.Services.AddSignalR();

var app = builder.Build();
var processor = app.Services.GetRequiredService<Portal2KioskMessagesReceiver>();
await processor.Run();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationhub");


app.Run();
