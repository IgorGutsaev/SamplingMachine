using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Products.Abstractions;
using MPT.Vending.Domains.Products.Infrastructure.Builders;
using MPT.Vending.Domains.Products.Infrastructure.Entities;
using MPT.Vending.Domains.Products.Infrastructure.Repositories;

namespace MPT.Vending.Domains.Products.Services
{
    internal class ProductService : IProductService
    {
        public event EventHandler<Product> onProductChanged;

        public ProductService(ProductRepository productRepository,
            ProductLocalizationRepository productLocalizationRepository,
            KioskProductLinkRepository kioskProductLinkRepository,
            PictureRepository pictureRepository) {
            _productRepository = productRepository;
            _productLocalizationRepository = productLocalizationRepository;
            _kioskProductLinkRepository = kioskProductLinkRepository;
            _pictureRepository = pictureRepository;
        }

        public async Task<Product?> GetAsync(string sku) {
            ProductBuilder builder = new ProductBuilder();

            var p = _productRepository.Get(x => x.Sku == sku).FirstOrDefault();
            if (p == null)
                return null;

            builder.WithData(p);
            var pic = _pictureRepository.Get(x => x.Id == p.PictureId).FirstOrDefault();
            if (pic != null) {
                string picture = await _pictureRepository.GetPictureAsBase64Async(pic.Uid);
                builder.WithPicture(picture);
            }

            IEnumerable<ProductLocalizationEntity> localizations = _productLocalizationRepository.Get(x => x.Attribute == "name" && x.ProductId == p.Id);
            builder.WithNames(localizations);

            return builder.Build();
        }

        public IEnumerable<Product> Get(IEnumerable<string> sku) {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Product> GetByFilter(string filter) {
            throw new NotImplementedException();
        }

        public async Task PutAsync(Product product) {
            if (string.IsNullOrWhiteSpace(product.Sku))
                throw new ArgumentException("Sku must be specified");

            ProductEntity? existed = _productRepository.Get(x => x.Sku == product.Sku).FirstOrDefault();

            ProductBuilder pBuilder = new ProductBuilder();

            if (existed != null) { // update product
                pBuilder.WithData(existed);

                if (_productLocalizationRepository.MergeNames(existed.Id, product.Names))
                    pBuilder.WithNames(product.Names);

                if (existed.PictureId.HasValue)
                    pBuilder.WithPicture(await _pictureRepository.GetPictureAsBase64Async(existed.PictureId.Value));

                onProductChanged?.Invoke(this, pBuilder.Build());
            }
            else { // add new product
                ProductEntity newProduct = _productRepository.Put(new ProductEntity { Sku = product.Sku.Trim().ToUpper() });
                pBuilder.WithData(newProduct);

                if (_productLocalizationRepository.MergeNames(newProduct.Id, product.Names))
                    pBuilder.WithNames(product.Names);

                onProductChanged?.Invoke(this, pBuilder.Build());
            }
        }

        public async Task PutPictureAsync(ProductPictureUpdateRequest request) {
            if (string.IsNullOrWhiteSpace(request.Sku))
                throw new ArgumentException("Sku must be specified");

            ProductEntity? product = _productRepository.Get(x => x.Sku == request.Sku).FirstOrDefault();
            if (product == null)
                throw new Exception("Product not found");

            string data = await _pictureRepository.GetPictureAsBase64Async(product.Id);

            if (data != request.Picture) {
                int? pictureId = await _pictureRepository.PutPictureAsBase64Async(product.PictureId, Convert.FromBase64String(request.Picture));
                if (pictureId != product.PictureId) { // save new picture if it has changed
                    product.PictureId = pictureId;
                    _productRepository.Put(product);
                }
            }
        }

        private readonly ProductRepository _productRepository;
        private readonly ProductLocalizationRepository _productLocalizationRepository;
        private readonly KioskProductLinkRepository _kioskProductLinkRepository;
        private readonly PictureRepository _pictureRepository;
    }
}
