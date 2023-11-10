using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Ordering.Abstractions;
using MPT.Vending.Domains.Ordering.Abstractions.Events;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Ordering.Services
{
    public class DemoProductService : IProductService
    {
        public event EventHandler<Product> onProductChanged;
        public event EventHandler<KioskProductsChangedEventArgs> onLinksChanged;

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

        public void LinkProduct(string kioskUid, string sku)
            => DemoData.Link(kioskUid, sku);

        public void UnlinkProduct(string kioskUid, string sku) {
            if (DemoData._kiosks.Any(x => x.UID == kioskUid) && DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks.Any(x => x.Product.Sku == sku))
                DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks =
                    DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks.Where(x => x.Product.Sku != sku);
        }

        public void ToggleProductLink(string kioskUid, string sku, bool disable) {
            Kiosk kiosk = DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid);
            if (kiosk != null) {
                kiosk.ProductLinks.FirstOrDefault(x => x.Product.Sku == sku).Disabled = disable;
                onLinksChanged?.Invoke(this, new KioskProductsChangedEventArgs { KioskUid = kioskUid, Links = kiosk.ProductLinks });
            }
        }

        public IEnumerable<KioskProductLink> GetKioskProductLinks(string kioskUid) => null;
    }
}