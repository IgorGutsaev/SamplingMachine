using Microsoft.EntityFrameworkCore;

namespace MPT.Vending.Domains.Products.Infrastructure.Configurations
{
    public static class ModelBuilderConfigurations
    {
        public static ModelBuilder WithCatalog(this ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new ProductConfiguration())
                .ApplyConfiguration(new ProductLocalizationConfiguration())
                .ApplyConfiguration(new KioskProductLinkConfiguration())
                .ApplyConfiguration(new KioskProductLinkViewConfiguration())
                .ApplyConfiguration(new PictureConfiguration());
            return modelBuilder;
        }
    }
}