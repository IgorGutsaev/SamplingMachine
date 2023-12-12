using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Identity.Abstractions;
using MPT.Vending.Domains.Kiosks.Abstractions;

namespace API.Controllers
{
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

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpGet("enable")]
        public void EnableProductLink(string uid)
            => _kioskService.EnableDisable(uid, true);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpGet("disable")]
        public void DisableProductLink(string uid)
            => _kioskService.EnableDisable(uid, false);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpGet("all")]
        public IEnumerable<Kiosk> GetAll()
            => _kioskService.GetAll();

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpPost]
        public Kiosk Post(string uid)
            => _kioskService.Add(uid);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpPut]
        public void Put([FromBody] Kiosk kiosk)
            => _kioskService.AddOrUpdate(kiosk);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpPost("credit")]
        public void SetCredit(string kioskUid, int credit, string sku = null)
            => _kioskService.SetCredit(kioskUid, sku, credit);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpPost("limit")]
        public void SetMaxCountPerTransaction(string kioskUid, string sku, int limit)
            => _kioskService.SetMaxCountPerTransaction(kioskUid, sku, limit);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpPut("media/{kioskUid}")]
        public void PutMedia([FromBody] IEnumerable<KioskMediaLink> links, string kioskUid)
            => _kioskService.SetMedia(kioskUid, links);

        [Authorize(Policy = IdentityData.KioskUserPolicyName)]
        [HttpPost("dispense")]
        public void Dispense([FromBody] DispensingEvent e)
            => _kioskService.Dispense(e.KioskUid, e.Address);

        private readonly IKioskService _kioskService;
        private readonly ILogger<KiosksController> _logger;
    }
}