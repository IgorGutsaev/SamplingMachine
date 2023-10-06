using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Products.Abstractions
{
    public interface IProductService
    {
        event EventHandler<Product> onProductChanged;
        Product Get(string sku);
        void Put(Product product);
        IEnumerable<Product> Get();
        IEnumerable<Product> Get(IEnumerable<string> sku);
        void PutPicture(ProductPictureUpdateRequest request);
    }
}
