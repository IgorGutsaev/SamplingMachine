using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Identity.Abstractions;
using MPT.Vending.Domains.Kiosks.Abstractions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        public CustomersController(IKioskService kioskService, ILogger<CustomersController> logger) {
            _kioskService = kioskService;
            _logger = logger;
        }

        [Authorize(Policy = IdentityData.KioskUserPolicyName)]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
            => request.Pin == "1234" ? Ok() : StatusCode(StatusCodes.Status401Unauthorized);

        private readonly IKioskService _kioskService;
        private readonly ILogger<CustomersController> _logger;
    }
}