using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Products.Abstractions;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Products.Services
{
    public class DemoProductService : IProductService
    {
        public event EventHandler<Product> onProductChanged;

        public Product Get(string sku)
            => DemoData._products.FirstOrDefault(x => x.Sku == sku);

        public void Put(Product product)
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

        public IEnumerable<Product> Get()
            => DemoData._products;

        public IEnumerable<Product> Get(IEnumerable<string> sku)
            => DemoData._products.Where(x => sku.Contains(x.Sku));

        public void PutPicture(ProductPictureUpdateRequest request)
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