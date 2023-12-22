using FutureTechniksProtocols;
using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using System.Text.Json;
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

    private readonly KioskService _kioskService;
    private readonly IDispenser _vendingMachineService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<KioskController> _logger;
}