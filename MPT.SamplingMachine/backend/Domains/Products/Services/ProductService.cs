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

            builder.WithSku(p);
            var pic = _pictureRepository.Get(x => x.Id == p.PictureId).FirstOrDefault();
            if (pic != null) {
                string picture = await _pictureRepository.GetPictureAsBase64(pic.Uid);
                builder.WithPicture(picture);
            }

            IEnumerable<ProductLocalizationEntity> localizations = _productLocalizationRepository.Get(x => x.ProductId == p.Id);
            builder.WithNames(localizations);

            return builder.Build();
        }

        public IEnumerable<Product> Get(IEnumerable<string> sku) {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Product> GetByFilter(string filter) {
            throw new NotImplementedException();
        }

        public void Put(Product product) {
            throw new NotImplementedException();
        }

        public void PutPicture(ProductPictureUpdateRequest request) {
            throw new NotImplementedException();
        }

        private readonly ProductRepository _productRepository;
        private readonly ProductLocalizationRepository _productLocalizationRepository;
        private readonly KioskProductLinkRepository _kioskProductLinkRepository;
        private readonly PictureRepository _pictureRepository;
    }
}
