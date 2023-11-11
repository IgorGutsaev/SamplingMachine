using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Advertisement.Infrastructure.Entities;

namespace MPT.Vending.Domains.Advertisement.Infrastructure.Configurations
{
    public class AdMediaConfiguration : IEntityTypeConfiguration<AdMediaEntity>
    {
        public void Configure(EntityTypeBuilder<AdMediaEntity> builder) {
            builder.ToTable("Media");
            builder.HasKey(x => x.Id);
        }
    }
}
