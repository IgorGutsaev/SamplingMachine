using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Products.Abstractions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("all")]
        public IEnumerable<ProductDto> GetAll()
            => _productService.Get();

        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
    }
}
