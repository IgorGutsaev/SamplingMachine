using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Configurations
{
    public class KioskConfiguration : IEntityTypeConfiguration<KioskEntity> {
        public void Configure(EntityTypeBuilder<KioskEntity> builder) {
            builder.ToTable("Kiosk");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Uid);
            builder.HasMany(x => x.Settings);//.WithOne(x => x.Kiosk);
            builder.HasMany(x => x.Links);
        }
    }
}