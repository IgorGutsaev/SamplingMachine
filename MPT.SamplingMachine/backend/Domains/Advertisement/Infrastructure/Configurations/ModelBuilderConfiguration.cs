using Microsoft.EntityFrameworkCore;

namespace MPT.Vending.Domains.Advertisement.Infrastructure.Configurations
{
    public static class ModelBuilderConfigurations
    {
        public static ModelBuilder WithAdvertisement(this ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new AdMediaConfiguration())
                .ApplyConfiguration(new KioskMediaLinkConfiguration())
                .ApplyConfiguration(new KioskMediaLinkViewConfiguration());
            return modelBuilder;
        }
    }
}
