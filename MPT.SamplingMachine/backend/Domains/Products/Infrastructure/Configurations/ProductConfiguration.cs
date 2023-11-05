using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPT.Vending.Domains.Products.Infrastructure.Entities;

namespace MPT.Vending.Domains.Products.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder) {
            builder.ToTable("Product");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Picture);
        }
    }
}