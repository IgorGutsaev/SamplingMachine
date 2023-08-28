using Filuet.Infrastructure.Abstractions.Enums;
using Filuet.Infrastructure.Abstractions.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingsController : ControllerBase
{
    public SettingsController(ILogger<SettingsController> logger)
    {
        _logger = logger;
    }

    [HttpGet("Languages")]
    public IEnumerable<dynamic> GetLanguages()
        => new [] { 
            new { value = Language.Hindi.GetName(), code = Language.Hindi.GetCode() }, 
            new { value = Language.English.GetName(), code = Language.English.GetCode() }
        };

    private readonly ILogger<SettingsController> _logger;
}