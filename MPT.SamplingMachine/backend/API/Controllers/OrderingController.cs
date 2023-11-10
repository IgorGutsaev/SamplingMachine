using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Ordering.Abstractions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderingController : ControllerBase
    {
        public OrderingController(IProductService productService, ITransactionService transactionService, ILogger<OrderingController> logger)
        {
            _productService = productService;
            _transactionService = transactionService;
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

        [HttpPut("transaction")]
        public void Transaction([FromBody] Transaction transaction)
            => _transactionService.Put(transaction);

        [HttpPost("transactions")]
        public IAsyncEnumerable<Transaction> GetTransactions([FromBody] TransactionRequest filter)
            => _transactionService.Get(filter);

        private readonly IProductService _productService;
        private readonly ITransactionService _transactionService;
        private readonly ILogger<OrderingController> _logger;
    }
}
