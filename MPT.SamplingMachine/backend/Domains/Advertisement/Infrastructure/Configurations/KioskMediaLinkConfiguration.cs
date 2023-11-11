using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Advertisement.Infrastructure.Entities;

namespace MPT.Vending.Domains.Advertisement.Infrastructure.Configurations
{
    public class KioskMediaLinkConfiguration : IEntityTypeConfiguration<KioskMediaLinkEntity>
    {
        public void Configure(EntityTypeBuilder<KioskMediaLinkEntity> builder) {
            builder.ToTable("KioskMediaLink");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Media);
        }
    }
}
