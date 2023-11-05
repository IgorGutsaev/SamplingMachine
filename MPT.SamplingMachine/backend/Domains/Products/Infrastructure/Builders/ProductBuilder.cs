using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Azure;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Products.Infrastructure.Entities;

namespace MPT.Vending.Domains.Products.Infrastructure.Builders
{
    public class ProductBuilder
    {
        private Product product = new Product();

        public ProductBuilder WithSku(ProductEntity entity) {
            product.Sku = entity.Sku;
            return this;
        }

        public ProductBuilder WithPicture(string picture) {
            product.Picture = picture;
            return this;
        }

        public ProductBuilder WithNames(IEnumerable<ProductLocalizationEntity> localizations) {
            var names = new List<LocalizedValue>();
            foreach (var l in localizations) {
                names.Add(LocalizedValue.Bind(l.Language, l.Value));
            }

            product.Names = names.ToArray();
            return this;
        }

        public Product Build() => product;
    }
}
