using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Abstractions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KiosksController : ControllerBase
    {
        public KiosksController(IKioskService kioskService, ILogger<KiosksController> logger)
        {
            _kioskService = kioskService;
            _logger = logger;
        }

        [HttpGet]
        public KioskDto Get(string uid)
            => _kioskService.Get(uid);

        [HttpGet("all")]
        public IEnumerable<KioskDto> GetAll()
            => _kioskService.GetAll();

        [HttpPost("link/disable")]
        public void DisableProductLink(string kioskUid, string sku)
            => _kioskService.DisableProductLink(kioskUid, sku);

        [HttpPost("link/enable")]
        public void EnableProductLink(string kioskUid, string sku)
            => _kioskService.EnableProductLink(kioskUid, sku);

        [HttpDelete("link")]
        public void DeleteProductLink(string kioskUid, string sku)
            => _kioskService.DeleteProductLink(kioskUid, sku);

        private readonly IKioskService _kioskService;
        private readonly ILogger<KiosksController> _logger;
    }
}