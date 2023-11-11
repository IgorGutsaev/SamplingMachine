using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Advertisement.Infrastructure.Entities;

namespace MPT.Vending.Domains.Advertisement.Infrastructure.Configurations
{
    public class KioskMediaLinkViewConfiguration : IEntityTypeConfiguration<KioskMediaLinkViewEntity>
    {
        public void Configure(EntityTypeBuilder<KioskMediaLinkViewEntity> builder) {
            builder.ToTable("KioskMediaLinkView");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Media);
        }
    }
}
