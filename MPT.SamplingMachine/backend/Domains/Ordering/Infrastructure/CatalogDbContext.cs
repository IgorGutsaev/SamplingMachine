using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Ordering.Infrastructure.Configurations;
using MPT.Vending.Domains.Ordering.Infrastructure.Entities;

namespace MPT.Vending.Domains.Ordering.Infrastructure
{
    public class CatalogDbContext : DbContext
    {
        public virtual DbSet<ProductEntity> Products { get; set; }
        public virtual DbSet<CustomerEntity> Customers { get; set; }
        public virtual DbSet<TransactionEntity> Transactions { get; set; }
        public virtual DbSet<TransactionItemEntity> TransactionItems { get; set; }
        public virtual DbSet<ProductLocalizationEntity> ProductLocalizations { get; set; }
        public virtual DbSet<KioskProductLinkEntity> KioskProductLinks { get; set; }
        public virtual DbSet<PictureEntity> Pictures { get; set; }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.WithCatalog();

            base.OnModelCreating(modelBuilder);
        }
    }
}