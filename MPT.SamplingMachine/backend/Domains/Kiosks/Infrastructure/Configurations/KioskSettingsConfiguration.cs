using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Configurations
{
    public class KioskSettingsConfiguration : IEntityTypeConfiguration<KioskSettingsEntity> {
        public void Configure(EntityTypeBuilder<KioskSettingsEntity> builder) {
            builder.ToTable("KioskSettings");
            builder.HasKey(x => x.Id).HasName("Id");
        }
    }
}