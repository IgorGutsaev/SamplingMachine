using Filuet.Infrastructure.Abstractions.Helpers;
using FutureTechniksProtocols;
using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using webapi.Services;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class KioskController : ControllerBase
{
    public KioskController(KioskService kioskService, IDispenser vendingMachineService, IConfiguration configuration, ILogger<KioskController> logger)
    {
        _kioskService = kioskService;
        _vendingMachineService = vendingMachineService;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpGet]
    public async Task<Kiosk> GetAsync()
        => await _kioskService.GetAsync();

    [HttpGet("cache/clear")]
    public void ClearCache()
        => _kioskService.ClearCache();

    [HttpPost("login")]
    public async Task<HttpResponseMessage> LoginAsync([FromBody] LoginRequest request)
        => await _kioskService.LoginAsync(request);
    
    [HttpPut("transaction")]
    public async Task PutTransactionAsync([FromBody] Transaction request)
        => await _kioskService.CommitTransactionAsync(request);

    [HttpPut("dispense")]
    public async Task DispenseAsync([FromBody] IEnumerable<TransactionProductLink> products) {
        Console.WriteLine(JsonSerializer.Serialize(products));
        try {
            IEnumerable<string> addresses = await _kioskService.GetDispensingList(products);
            await _vendingMachineService.SendExtractAsync(addresses.Select(x => Convert.ToInt32(x)));
        }
        catch (Exception ex) {

            Console.WriteLine(ex.Message);
        }
    }

    [HttpPost("loginService")]
    public IActionResult LoginService([FromBody] ServiceLoginRequest loginRequest) {
        if (loginRequest.Pin == "1234")
            return Ok(new { replenishmentUrl = new Uri(new Uri(_configuration["Portal"]), $"replenishment/{_configuration["KioskUid"]}") });

        return StatusCode(StatusCodes.Status401Unauthorized);
    }

    [HttpPost("log/{logLevel}")]
    public async Task<IActionResult> LogAsync(int logLevel) {
        try {
            using StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8);
            string text = await reader.ReadToEndAsync();
            if (string.IsNullOrEmpty(text) || text.Length < 3)
                return Ok();

            if (text.Length > 10000)
                text = text.Substring(0, 10000) + "...";

            text = text.Replace("\\\"", "\"");

            var regex = new Regex("\"cvv\":\"(.*?)\"");
            var matches = regex.Matches(text);
            if (matches.Count > 0) {
                string stars = "\"cvv\":\"" + new string('*', matches.FirstOrDefault().Groups[1].Value.Length) + "\"";
                text = regex.Replace(text, stars);
            }

            LogLevel level = FluentSwitch.On(logLevel).Case(0).Then(LogLevel.Information)
                .Case(1).Then(LogLevel.Debug)
                .Case(-1).Then(LogLevel.Error).Default(LogLevel.Trace);

            switch (level) {
                case LogLevel.Information:
                    _logger.LogInformation(text);
                    break;
                case LogLevel.Debug:
                    _logger.LogDebug(text);
                    break;
                case LogLevel.Error:
                    _logger.LogError(text);
                    break;
            }

           // OnLogEvent?.Invoke(this, new LogEventArgs { Level = level, Message = text.Trim() });
        }
        catch (Exception ex) {
            _logger.LogError($"Log Controller error: {ex.Message}");
        }
        return Ok();
    }

    private readonly KioskService _kioskService;
    private readonly IDispenser _vendingMachineService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<KioskController> _logger;
}