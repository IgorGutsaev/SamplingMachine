using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Ordering.Infrastructure.Entities;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Builders
{
    public class ProductBuilder
    {
        private Product product = new Product();

        /// <summary>
        /// Set product sku
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ProductBuilder WithData(ProductEntity entity) {
            product.Sku = entity.Sku;
            return this;
        }

        /// <summary>
        /// Set product localized names
        /// </summary>
        /// <param name="localizations"></param>
        /// <returns></returns>
        public ProductBuilder WithNames(IEnumerable<ProductLocalizationEntity> localizations) {
            var names = new List<LocalizedValue>();
            foreach (var l in localizations) {
                names.Add(LocalizedValue.Bind(l.Language, l.Value));
            }

            product.Names = names.ToArray();
            return this;
        }

        /// <summary>
        /// Set product localized names
        /// </summary>
        /// <param name="localizations"></param>
        /// <returns></returns>
        public ProductBuilder WithNames(IEnumerable<LocalizedValue>? localizations) {
            product.Names = localizations;
            return this;
        }

        public ProductBuilder WithPicture(string picture) {
            product.Picture = picture;
            return this;
        }

        public Product Build() => product;
    }
}
