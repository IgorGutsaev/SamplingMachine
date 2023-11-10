using Microsoft.EntityFrameworkCore;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Configurations
{
    public static class ModelBuilderConfigurations
    {
        public static ModelBuilder WithCatalog(this ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new ProductConfiguration())
                .ApplyConfiguration(new ProductLocalizationConfiguration())
                .ApplyConfiguration(new KioskProductLinkConfiguration())
                .ApplyConfiguration(new PictureConfiguration())
                .ApplyConfiguration(new CustomerConfiguration())
                .ApplyConfiguration(new TransactionConfiguration())
                .ApplyConfiguration(new TransactionItemsConfiguration());
            return modelBuilder;
        }
    }
}