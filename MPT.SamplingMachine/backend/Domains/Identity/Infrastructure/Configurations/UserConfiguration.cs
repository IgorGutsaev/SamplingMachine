using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Identity.Infrastructure.Entities;

namespace MPT.Vending.Domains.Identity.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity> {
        public void Configure(EntityTypeBuilder<UserEntity> builder) {
            builder.ToTable("User");
            builder.HasKey(x => x.Id);
            builder.HasMany(x => x.Claims);
        }
    }
}