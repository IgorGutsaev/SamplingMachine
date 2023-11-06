using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Products.Abstractions;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Products.Services
{
    public class DemoProductService : IProductService
    {
        public event EventHandler<Product> onProductChanged;

        public async Task<Product?> GetAsync(string sku)
            => DemoData._products.FirstOrDefault(x => x.Sku == sku);

        public async Task PutAsync(Product product)
        {
            Product existed = DemoData._products.FirstOrDefault(x => x.Sku == product.Sku);

            if (existed != null)
            {
                existed.Names = product.Names;
                
                onProductChanged?.Invoke(this, existed);
            }
            else
            {
                List<Product> products = DemoData._products;
                products.Add(product);
                DemoData._products = products;
                onProductChanged?.Invoke(this, product);
            }
        }

        public async IAsyncEnumerable<Product> GetByFilter(string filter)
        {
            foreach (var p in DemoData._products.Where(x => string.IsNullOrWhiteSpace(filter) || x.Sku.Contains(filter, StringComparison.InvariantCultureIgnoreCase) || (x.Names?.Any(n => n.Value.Contains(filter, StringComparison.InvariantCultureIgnoreCase)) ?? false)))
                yield return p;
        }

        public IEnumerable<Product> Get(IEnumerable<string> sku)
            => DemoData._products.Where(x => sku.Contains(x.Sku));

        public async Task PutPictureAsync(ProductPictureUpdateRequest request)
        {
            Product p = DemoData._products.FirstOrDefault(x => x.Sku == request.Sku);
            if (p != null)
            {
                if (p.Picture != request.Picture && !string.IsNullOrWhiteSpace(request.Picture)) // picture has changed
                    p.Picture = request.Picture;

                onProductChanged?.Invoke(this, p);
            }
        }
    }
}