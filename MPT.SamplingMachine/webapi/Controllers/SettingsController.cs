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

    [HttpPost("languages")]
    public IEnumerable<dynamic> GetLanguages([FromBody] IEnumerable<Language> languages)
    {
         foreach (var l in languages)
            yield return new { value = l.GetName(), code = l.GetCode() };
    }

    private readonly ILogger<SettingsController> _logger;
}