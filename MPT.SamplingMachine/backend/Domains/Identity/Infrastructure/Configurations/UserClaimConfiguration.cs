using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Identity.Infrastructure.Entities;

namespace MPT.Vending.Domains.Identity.Infrastructure.Configurations
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaimEntity> {
        public void Configure(EntityTypeBuilder<UserClaimEntity> builder) {
            builder.ToTable("UserClaim");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Claim);
        }
    }
}