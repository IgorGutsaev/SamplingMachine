using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Products.Abstractions
{
    public interface IProductService
    {
        Product Get(string sku);
        IEnumerable<Product> Get();
        IEnumerable<Product> Get(IEnumerable<string> sku);
    }
}
