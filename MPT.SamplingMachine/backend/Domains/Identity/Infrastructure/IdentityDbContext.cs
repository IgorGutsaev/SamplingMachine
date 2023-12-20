using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Identity.Infrastructure.Configurations;
using MPT.Vending.Domains.Identity.Infrastructure.Entities;

namespace MPT.Vending.Domains.Kiosks.Infrastructure
{
    public class IdentityDbContext : DbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; }
        public virtual DbSet<ClaimEntity> Claims { get; set; }
        public virtual DbSet<UserClaimEntity> UserClaims { get; set; }

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.WithIdentity();

            base.OnModelCreating(modelBuilder);
        }
    }
}