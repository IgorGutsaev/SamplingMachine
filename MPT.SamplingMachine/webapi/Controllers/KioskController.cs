using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using webapi.Services;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class KioskController : ControllerBase
{
    public KioskController(KioskService kioskService, ILogger<KioskController> logger)
    {
        _kioskService = kioskService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<KioskDto> GetAsync()
        => await _kioskService.GetAsync();

    private readonly KioskService _kioskService;
    private readonly ILogger<KioskController> _logger;
}