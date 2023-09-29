using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using webapi.Services;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class KiosksController : ControllerBase
{
    public KiosksController(KioskService kioskService, ILogger<KiosksController> logger)
    {
        _kioskService = kioskService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<Kiosk> GetAsync()
        => await _kioskService.GetAsync();

    [HttpPost("login")]
    public async Task<HttpResponseMessage> LoginAsync([FromBody] LoginRequest request)
        => await _kioskService.LoginAsync(request);

    [HttpPut("session")]
    public async Task PutSessionAsync([FromBody] Session request)
        => await _kioskService.CommitSessionAsync(request);

    private readonly KioskService _kioskService;
    private readonly ILogger<KiosksController> _logger;
}