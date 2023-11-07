using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Products.Abstractions
{
    public interface IProductService
    {
        event EventHandler<Product> onProductChanged;
        Task<Product?> GetAsync(string sku);
        IAsyncEnumerable<Product> GetAsync(IEnumerable<string> sku);
        IAsyncEnumerable<Product> GetByFilterAsync(string filter);
        Task PutAsync(Product product);
        Task PutPictureAsync(ProductPictureUpdateRequest request);
        IEnumerable<string> GetKiosksWithSku(string sku);
    }
}