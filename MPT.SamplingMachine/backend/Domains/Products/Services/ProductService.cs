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
            KioskProductLinkViewRepository kioskProductLinkViewRepository,
            PictureRepository pictureRepository) {
            _productRepository = productRepository;
            _productLocalizationRepository = productLocalizationRepository;
            _kioskProductLinkRepository = kioskProductLinkRepository;
            _kioskProductLinkViewRepository = kioskProductLinkViewRepository;
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

        public async IAsyncEnumerable<Product> GetAsync(IEnumerable<string> sku) {
            IEnumerable<ProductEntity> entities = _productRepository.Get(x => sku.Contains(x.Sku));

            await foreach (var t in GetProductsAsync(entities))
                yield return t;
        }

        public async IAsyncEnumerable<Product> GetByFilterAsync(string filter) {
            IEnumerable<int> suitableProducts = _productLocalizationRepository.FindProductsByFilter(filter);

            IEnumerable<ProductEntity> entities = _productRepository.Get(x => string.IsNullOrWhiteSpace(filter) ||
                x.Sku.Contains(filter, StringComparison.InvariantCultureIgnoreCase) || suitableProducts.Contains(x.Id)).ToList();

            await foreach (var t in GetProductsAsync(entities))
                yield return t;
        }

        private async IAsyncEnumerable<Product> GetProductsAsync(IEnumerable<ProductEntity> entities) {
            IEnumerable<ProductLocalizationEntity> localizations = _productLocalizationRepository.Get(x => x.Attribute == "name" && entities.Select(p => p.Id).Contains(x.ProductId)).ToList();

            IEnumerable<PictureEntity> pictures = _pictureRepository.Get(x => entities.Select(p => p.PictureId).Contains(x.Id)).ToList();
            foreach (var p in entities) {
                ProductBuilder builder = new ProductBuilder();
                builder.WithData(p);
                var pic = pictures.FirstOrDefault(x => x.Id == p.PictureId);
                if (pic != null) {
                    string picture = await _pictureRepository.GetPictureAsBase64Async(pic.Uid);
                    builder.WithPicture(picture);
                }

                IEnumerable<ProductLocalizationEntity> pLocalizations = localizations.Where(x => x.ProductId == p.Id).ToList();
                builder.WithNames(pLocalizations);

                yield return builder.Build();
            }
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
                _productLocalizationRepository.MergeNames(newProduct.Id, product.Names);
            }
        }

        public async Task PutPictureAsync(ProductPictureUpdateRequest request) {
            if (string.IsNullOrWhiteSpace(request.Sku))
                throw new ArgumentException("Sku must be specified");

            ProductEntity? product = _productRepository.Get(x => x.Sku == request.Sku).FirstOrDefault();
            if (product == null)
                throw new Exception("Product not found");

            string? data = null;

            if (product.PictureId.HasValue) {
                PictureEntity? pe = _pictureRepository.Get(x => x.Id == product.PictureId).FirstOrDefault();
                if (pe != null)
                    data = await _pictureRepository.GetPictureAsBase64Async(pe.Uid);
            }

            if (data != request.Picture) {
                PictureEntity? picture = await _pictureRepository.PutPictureAsBase64Async(product.PictureId, Convert.FromBase64String(request.Picture));
                if (picture != null && picture.Id != product.PictureId) { // save new picture if it has changed
                    product.PictureId = picture.Id;
                    product.Picture = picture;
                    _productRepository.Put(product);
                }
            }
        }

        public IEnumerable<string> GetKiosksWithSku(string sku) { 
            ProductEntity? p = _productRepository.Get(x => x.Sku == sku).FirstOrDefault();
            if (p == null)
                return new List<string>();

            return _kioskProductLinkViewRepository.Get(x => x.ProductId == p.Id).Select(x => x.KioskUid).ToList().Distinct();
        }

        private readonly ProductRepository _productRepository;
        private readonly ProductLocalizationRepository _productLocalizationRepository;
        private readonly KioskProductLinkRepository _kioskProductLinkRepository;
        private readonly KioskProductLinkViewRepository _kioskProductLinkViewRepository;
        private readonly PictureRepository _pictureRepository;
    }
}
