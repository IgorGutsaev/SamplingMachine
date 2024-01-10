using Filuet.Hardware.Dispensers.Abstractions.Models;
using MessagingServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Identity.Abstractions;
using MPT.Vending.Domains.Kiosks.Abstractions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReplenishmentController : ControllerBase
    {
        public ReplenishmentController(IReplenishmentService replenishmentService, StockCache stockBalance, ILogger<ReplenishmentController> logger) {
            _replenishmentService = replenishmentService;
            _stockBalance = stockBalance;
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

        [HttpGet("stock")]
        public IEnumerable<KioskStock>? GetStock()
            => _stockBalance.Stock;

        private readonly IReplenishmentService _replenishmentService;
        private readonly StockCache _stockBalance;
        private readonly ILogger<ReplenishmentController> _logger;
    }
}
