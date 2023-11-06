using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Products.Abstractions
{
    public interface IProductService
    {
        event EventHandler<Product> onProductChanged;
        Task<Product?> GetAsync(string sku);
        Task PutAsync(Product product);
        IAsyncEnumerable<Product> GetByFilter(string filter);
        IEnumerable<Product> Get(IEnumerable<string> sku);
        Task PutPictureAsync(ProductPictureUpdateRequest request);
    }
}