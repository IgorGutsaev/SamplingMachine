using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Advertisement.Infrastructure.Configurations;
using MPT.Vending.Domains.Advertisement.Infrastructure.Entities;

namespace MPT.Vending.Domains.Advertisement.Infrastructure
{
    public class AdvertisementDbContext : DbContext
    {
        public virtual DbSet<AdMediaEntity> Media { get; set; }
        public virtual DbSet<KioskMediaLinkEntity> KioskMediaLinks { get; set; }
        public virtual DbSet<KioskMediaLinkViewEntity> KioskMediaLinkView { get; set; }

        public AdvertisementDbContext(DbContextOptions<AdvertisementDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.WithAdvertisement();

            base.OnModelCreating(modelBuilder);
        }
    }
}