using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;

namespace MPT.Vending.Domains.Products.Infrastructure.Configurations
{
    public class KioskProductLinkViewConfiguration : IEntityTypeConfiguration<KioskProductLinkViewEntity>
    {
        public void Configure(EntityTypeBuilder<KioskProductLinkViewEntity> builder) {
            builder.ToTable("KioskProductLinkView");
        }
    }
}