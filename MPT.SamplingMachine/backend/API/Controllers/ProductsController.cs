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
        public Product Get(string sku)
            => _productService.Get(sku);

        [HttpPost]
        public IEnumerable<Product> Get([FromBody] ProductRequest request)
            => _productService.Get(request.Sku);

        [HttpPut]
        public void Put([FromBody] Product product)
            => _productService.Put(product);

        [HttpGet("all")]
        public IEnumerable<Product> GetAll()
            => _productService.Get();

        [HttpPut("picture")]
        public void PutPicture([FromBody] ProductPictureUpdateRequest request)
            => _productService.PutPicture(request);

        [HttpPut("session")]
        public void GetSessions([FromBody] Session session)
            => _sessionService.Put(session);

        [HttpPost("sessions")]
        public IEnumerable<Session> GetSessions([FromBody] SessionsRequest request)
            => _sessionService.Get(request);

        private readonly IProductService _productService;
        private readonly ISessionService _sessionService;
        private readonly ILogger<ProductsController> _logger;
    }
}
