using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.Products.Abstractions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KiosksController : ControllerBase
    {
        public KiosksController(IKioskService kioskService, IProductService productService, ILogger<KiosksController> logger) {
            _kioskService = kioskService;
            _productService = productService;
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
        public void SetMaxCountPerSession(string kioskUid, string sku, int limit)
            => _kioskService.SetMaxCountPerSession(kioskUid, sku, limit);

        [HttpPut("media/{kioskUid}")]
        public void PutMedia([FromBody] IEnumerable<KioskMediaLink> links, string kioskUid)
            => _kioskService.SetMedia(kioskUid, links);

        private readonly IKioskService _kioskService;
        private readonly IProductService _productService;
        private readonly ILogger<KiosksController> _logger;
    }
}