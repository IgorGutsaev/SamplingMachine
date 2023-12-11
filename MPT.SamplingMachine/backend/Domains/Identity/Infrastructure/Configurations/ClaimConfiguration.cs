using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Identity.Infrastructure.Entities;

namespace MPT.Vending.Domains.Identity.Infrastructure.Configurations
{
    public class ClaimConfiguration : IEntityTypeConfiguration<ClaimEntity> {
        public void Configure(EntityTypeBuilder<ClaimEntity> builder) {
            builder.ToTable("Claim");
            builder.HasKey(x => x.Id);
        }
    }
}