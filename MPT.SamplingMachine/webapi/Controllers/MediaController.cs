using Microsoft.AspNetCore.Mvc;
using MPT.SamplingMachine.ApiClient;
using System.IO;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class MediaController : ControllerBase
{
    public MediaController(SamplingMachineApiClient client, ILogger<MediaController> logger)
    {
        _client = client;
        _logger = logger;
    }

    [HttpGet("find/{format}/{hash}")]
    public async Task<IActionResult> GetMedia(string hash, string format) {
        if (string.Equals(hash, "undefined", StringComparison.InvariantCultureIgnoreCase))
            return StatusCode(StatusCodes.Status204NoContent);

        string localStorage = $"assets/{format}/";
        byte[] data;
        if (System.IO.File.Exists($"{localStorage}{hash}"))
            data = System.IO.File.ReadAllBytes($"{localStorage}{hash}");
        else {
            data = await _client.DownloadMediaAsync(hash, format);
            if (!Directory.Exists(localStorage))
                Directory.CreateDirectory(localStorage);

            System.IO.File.WriteAllBytes($"{localStorage}{hash}", data);
            return File(data, "application/octet-stream", $"{hash}.{format}");
        } 

        return File(data, "application/octet-stream", $"{hash}.{format}");
    }

    private readonly SamplingMachineApiClient _client;
    private readonly ILogger<MediaController> _logger;
}