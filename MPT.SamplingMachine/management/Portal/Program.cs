using Blazored.LocalStorage;
using Filuet.Infrastructure.Abstractions.Converters;
using MPT.SamplingMachine.ApiClient;
using Portal.Hubs;
using Portal.StateContainers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<KioskStateContainer>();
builder.Services.AddSingleton<ProductStateContainer>();
builder.Services.AddSingleton<TransactionStateContainer>();
builder.Services.AddSingleton<HookHub>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AppState>();
builder.Services.AddScoped<BrowserService>();

builder.Services.AddControllers()
    .AddJsonOptions(opts => {
        opts.JsonSerializerOptions.Converters.Add(new CurrencyJsonConverter());
        opts.JsonSerializerOptions.Converters.Add(new CountryJsonConverter());
        opts.JsonSerializerOptions.Converters.Add(new LanguageJsonConverter());
        opts.JsonSerializerOptions.Converters.Add(new N2JsonConverter());
    });

builder.Services.AddSingleton(sp => new SamplingMachineApiClient(builder.Configuration["ApiUrl"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapHub<HookHub>("/hookhub");
app.MapControllers();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();