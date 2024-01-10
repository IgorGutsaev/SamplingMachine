using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Identity.Abstractions;
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

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpGet]
        public async Task<Product?> Get(string sku)
            => await _productService.GetAsync(sku);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpPost]
        public IAsyncEnumerable<Product> Get([FromBody] ProductRequest request)
            => _productService.GetAsync(request.Sku);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpPut]
        public void Put([FromBody] Product product)
            => _productService.PutAsync(product);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpGet("all")]
        public IAsyncEnumerable<Product> GetAll(string filter = "")
            => _productService.GetByFilterAsync(filter);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpPut("link")]
        public void LinkProduct(string kioskUid, string sku)
            => _productService.LinkProduct(kioskUid, sku);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpDelete("unlink")]
        public void UnlinkProduct(string kioskUid, string sku)
            => _productService.UnlinkProduct(kioskUid, sku);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpPost("link/enable")]
        public void EnableProductLink(string kioskUid, string sku)
            => _productService.ToggleProductLink(kioskUid, sku, false);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpPost("link/disable")]
        public void DisableProductLink(string kioskUid, string sku)
            => _productService.ToggleProductLink(kioskUid, sku, true);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpPut("picture")]
        public async Task PutPicture([FromBody] ProductPictureUpdateRequest request)
            => await _productService.PutPictureAsync(request);

        [Authorize(Policy = IdentityData.KioskUserPolicyName)]
        [HttpPut("transaction")]
        public void Transaction([FromBody] Transaction transaction)
            => _transactionService.Put(transaction);

        [Authorize(Policy = IdentityData.ManagerPolicyName)]
        [HttpPost("transactions")]
        public IAsyncEnumerable<Transaction> GetTransactions([FromBody] TransactionRequest filter)
            => _transactionService.Get(filter);

        private readonly IProductService _productService;
        private readonly ITransactionService _transactionService;
        private readonly ILogger<OrderingController> _logger;
    }
}
