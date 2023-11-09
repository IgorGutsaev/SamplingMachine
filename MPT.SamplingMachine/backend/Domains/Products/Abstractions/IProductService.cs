using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Products.Abstractions.Events;

namespace MPT.Vending.Domains.Products.Abstractions
{
    public interface IProductService
    {
        event EventHandler<Product> onProductChanged;

        event EventHandler<KioskProductsChangedEventArgs> onLinksChanged;
        Task<Product?> GetAsync(string sku);
        IAsyncEnumerable<Product> GetAsync(IEnumerable<string> sku);
        IAsyncEnumerable<Product> GetByFilterAsync(string filter);
        Task PutAsync(Product product);
        Task PutPictureAsync(ProductPictureUpdateRequest request);
        void LinkProduct(string kioskUid, string sku);
        void UnlinkProduct(string kioskUid, string sku);
        void ToggleProductLink(string kioskUid, string sku, bool disable);
    }
}