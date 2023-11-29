using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Abstractions;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class KiosksController : ControllerBase
    {
        public KiosksController(IKioskService kioskService, ILogger<KiosksController> logger) {
            _kioskService = kioskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<Kiosk> GetAsync(string uid)
            => _kioskService.Get(uid);

        [HttpGet("enable")]
        public void EnableProductLink(string uid)
            => _kioskService.EnableDisable(uid, true);

        [HttpGet("disable")]
        public void DisableProductLink(string uid)
            => _kioskService.EnableDisable(uid, false);

        [HttpGet("all")]
        public IEnumerable<Kiosk> GetAll()
            => _kioskService.GetAll();

        [HttpPost]
        public Kiosk Post(string uid)
            => _kioskService.Add(uid);

        [HttpPut]
        public void Put([FromBody] Kiosk kiosk)
            => _kioskService.AddOrUpdate(kiosk);

        [HttpPost("credit")]
        public void SetCredit(string kioskUid, int credit, string sku = null)
            => _kioskService.SetCredit(kioskUid, sku, credit);

        [HttpPost("limit")]
        public void SetMaxCountPerTransaction(string kioskUid, string sku, int limit)
            => _kioskService.SetMaxCountPerTransaction(kioskUid, sku, limit);

        [HttpPut("media/{kioskUid}")]
        public void PutMedia([FromBody] IEnumerable<KioskMediaLink> links, string kioskUid)
            => _kioskService.SetMedia(kioskUid, links);

        [HttpPost("dispense")]
        public void Dispense([FromBody] DispensingEvent e)
            => _kioskService.Dispense(e.KioskUid, e.Address);

        private readonly IKioskService _kioskService;
        private readonly ILogger<KiosksController> _logger;
    }
}