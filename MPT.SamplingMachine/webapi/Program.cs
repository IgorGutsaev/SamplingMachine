using FutureTechniksProtocols;
using Filuet.Infrastructure.DataProvider;
using Filuet.Infrastructure.DataProvider.Interfaces;
using Microsoft.AspNetCore.SignalR;
using MPT.SamplingMachine.ApiClient;
using System.Net;
using webapi.Communication;
using webapi.Services;

var builder = WebApplication.CreateBuilder(args);

#if !DEBUG
builder.WebHost.ConfigureKestrel((context, serverOptions) => {
    serverOptions.Listen(IPAddress.Loopback, 7244, listenOptions => {
        listenOptions.UseHttps();
    });
});
#endif

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMemoryCachingService, MemoryCachingService>();
builder.Services.AddSingleton(sp =>
    new SamplingMachineApiClient(x => {
        x.Url = builder.Configuration["ApiUrl"] ?? "https://ogmento-api.azurewebsites.net/";
        x.Email = "smytten1@filuet.com";
        x.Password = "87UsQaYnXB";
    }
));

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
    int portId = Convert.ToInt32(sp.GetService<IConfiguration>()["Hardware:DispensingUnit:SerialPort"]);
    Console.WriteLine($"Com port: {portId}");

    IDispenser dispenser = portId > 0 ? new VmcDispenser(portId) : new VmcEmulator();

    dispenser.onEvent += (sender, e) => {
        Console.WriteLine($"{DateTime.Now:T}: {(e.IsCommand ? ">" : "<")} {e.ResponseToString}" + (!string.IsNullOrWhiteSpace(e.Comment) ? $" ({e.Comment})" : string.Empty));
    };

    KioskService kioskService = sp.GetRequiredService<KioskService>();

    dispenser.onDispensing += (sender, e) => {
        kioskService.DispenseAsync(e.MotorId);
        Console.WriteLine($"Dispensing event. Motor: {e.MotorId}, Dispensed: {e.Dispensed}");
    };

    return dispenser;
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
//if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationhub");

app.Run();