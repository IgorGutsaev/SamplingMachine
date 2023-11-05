using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MPT.Vending.API.Dto;
using webapi.Services;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class KioskController : ControllerBase
{
    public KioskController(KioskService kioskService, IConfiguration configuration, ILogger<KioskController> logger)
    {
        _kioskService = kioskService;
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

    [HttpPut("session")]
    public async Task PutSessionAsync([FromBody] Session request)
        => await _kioskService.CommitSessionAsync(request);

    [HttpPost("loginService")]
    public IActionResult LoginService([FromBody] ServiceLoginRequest loginRequest) {
        if (loginRequest.Pin == "1234")
            return Ok(new { replenishmentUrl = new Uri(new Uri(_configuration["Portal"]), $"replenishment/{_configuration["KioskUid"]}") });

        return StatusCode(StatusCodes.Status401Unauthorized);
    }

    private readonly KioskService _kioskService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<KioskController> _logger;
}