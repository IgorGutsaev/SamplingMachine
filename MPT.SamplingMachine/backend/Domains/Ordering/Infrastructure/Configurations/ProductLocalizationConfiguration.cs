using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPT.Vending.Domains.Ordering.Infrastructure.Entities;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Configurations
{
    public class ProductLocalizationConfiguration : IEntityTypeConfiguration<ProductLocalizationEntity>
    {
        public void Configure(EntityTypeBuilder<ProductLocalizationEntity> builder) {
            builder.ToTable("ProductLocalization");
            builder.HasKey(x => x.Id);
        }
    }
}