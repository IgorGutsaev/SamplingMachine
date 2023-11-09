using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Products.Abstractions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public ProductsController(IProductService productService, ISessionService sessionService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _sessionService = sessionService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<Product?> Get(string sku)
            => await _productService.GetAsync(sku);

        [HttpPost]
        public IAsyncEnumerable<Product> Get([FromBody] ProductRequest request)
            => _productService.GetAsync(request.Sku);

        [HttpPut]
        public void Put([FromBody] Product product)
            => _productService.PutAsync(product);

        [HttpGet("all")]
        public IAsyncEnumerable<Product> GetAll(string filter = "")
            => _productService.GetByFilterAsync(filter);


        [HttpPut("link")]
        public void LinkProduct(string kioskUid, string sku)
            => _productService.LinkProduct(kioskUid, sku);

        [HttpDelete("unlink")]
        public void UnlinkProduct(string kioskUid, string sku)
            => _productService.UnlinkProduct(kioskUid, sku);


        [HttpPost("link/enable")]
        public void EnableProductLink(string kioskUid, string sku)
            => _productService.ToggleProductLink(kioskUid, sku, false);

        [HttpPost("link/disable")]
        public void DisableProductLink(string kioskUid, string sku)
            => _productService.ToggleProductLink(kioskUid, sku, true);

        [HttpPut("picture")]
        public async Task PutPicture([FromBody] ProductPictureUpdateRequest request)
            => await _productService.PutPictureAsync(request);

        [HttpPut("session")]
        public void GetSessions([FromBody] Session session)
            => _sessionService.Put(session);

        [HttpPost("sessions")]
        public IAsyncEnumerable<Session> GetSessions([FromBody] SessionsRequest filter)
            => _sessionService.Get(filter);

        private readonly IProductService _productService;
        private readonly ISessionService _sessionService;
        private readonly ILogger<ProductsController> _logger;
    }
}
