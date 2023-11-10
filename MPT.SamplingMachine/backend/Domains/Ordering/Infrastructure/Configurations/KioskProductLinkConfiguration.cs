using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPT.Vending.Domains.Ordering.Infrastructure.Entities;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Configurations
{
    public class KioskProductLinkConfiguration : IEntityTypeConfiguration<KioskProductLinkEntity>
    {
        public void Configure(EntityTypeBuilder<KioskProductLinkEntity> builder) {
            builder.ToTable("KioskProductLink");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Product);
        }
    }
}