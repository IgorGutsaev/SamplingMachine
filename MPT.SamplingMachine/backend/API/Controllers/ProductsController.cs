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
        public IEnumerable<Product> Get([FromBody] ProductRequest request)
            => _productService.Get(request.Sku);

        [HttpPut]
        public void Put([FromBody] Product product)
            => _productService.Put(product);

        [HttpGet("all")]
        public IAsyncEnumerable<Product> GetAll(string filter = "")
            => _productService.GetByFilter(filter);

        [HttpPut("picture")]
        public void PutPicture([FromBody] ProductPictureUpdateRequest request)
            => _productService.PutPicture(request);

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
