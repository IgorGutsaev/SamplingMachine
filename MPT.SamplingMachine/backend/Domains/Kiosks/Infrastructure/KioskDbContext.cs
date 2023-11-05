using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Kiosks.Infrastructure.Configurations;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;

namespace MPT.Vending.Domains.Kiosks.Infrastructure
{
    public class KioskDbContext : DbContext
    {
        public virtual DbSet<KioskEntity> Kiosks { get; set; }
        public virtual DbSet<KioskSettingsEntity> KioskSettings { get; set; }

        public KioskDbContext(DbContextOptions<KioskDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.WithKiosk();

            base.OnModelCreating(modelBuilder);
        }
    }
}