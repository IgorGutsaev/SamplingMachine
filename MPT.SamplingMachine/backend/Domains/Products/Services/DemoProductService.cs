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

        public async IAsyncEnumerable<Product> GetAsync(IEnumerable<string> sku) {
            foreach (var p in DemoData._products.Where(x => sku.Contains(x.Sku)))
                yield return p;
        }

        public async IAsyncEnumerable<Product> GetByFilterAsync(string filter) {
            foreach (var p in DemoData._products.Where(x => string.IsNullOrWhiteSpace(filter) || x.Sku.Contains(filter, StringComparison.InvariantCultureIgnoreCase) || (x.Names?.Any(n => n.Value.Contains(filter, StringComparison.InvariantCultureIgnoreCase)) ?? false)))
                yield return p;
        }

        public async Task PutAsync(Product product) {
            Product existed = DemoData._products.FirstOrDefault(x => x.Sku == product.Sku);

            if (existed != null) {
                existed.Names = product.Names;

                onProductChanged?.Invoke(this, existed);
            }
            else {
                List<Product> products = DemoData._products;
                products.Add(product);
                DemoData._products = products;
                onProductChanged?.Invoke(this, product);
            }
        }

        public async Task PutPictureAsync(ProductPictureUpdateRequest request) {
            Product p = DemoData._products.FirstOrDefault(x => x.Sku == request.Sku);
            if (p != null) {
                if (p.Picture != request.Picture && !string.IsNullOrWhiteSpace(request.Picture)) // picture has changed
                    p.Picture = request.Picture;

                onProductChanged?.Invoke(this, p);
            }
        }
        public IEnumerable<string> GetKiosksWithSku(string sku)
            => DemoData._kiosks.Where(x => x.ProductLinks.Any(l => l.Product.Sku == sku)).Select(x => x.UID).Distinct();
    }
}