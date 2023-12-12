using Filuet.Hardware.Dispensers.Abstractions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPT.Vending.Domains.Identity.Abstractions;
using MPT.Vending.Domains.Kiosks.Abstractions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplenishmentController : ControllerBase
    {
        public ReplenishmentController(IReplenishmentService replenishmentService, ILogger<ReplenishmentController> logger) {
            _replenishmentService = replenishmentService;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid">kiosk uid</param>
        /// <returns></returns>
        [HttpGet("planogram")]
        public PoG GetPlanogram(string uid)
            => _replenishmentService.GetPlanogram(uid);

        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        [HttpPut("planogram")]
        public void PutPlanogram([FromBody] PoG planogram, string uid)
            => _replenishmentService.PutPlanogram(uid, planogram);

        private readonly IReplenishmentService _replenishmentService;
        private readonly ILogger<ReplenishmentController> _logger;
    }
}
