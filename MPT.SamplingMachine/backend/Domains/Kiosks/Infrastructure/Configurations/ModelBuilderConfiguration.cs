using Microsoft.EntityFrameworkCore;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Configurations
{
    public static class ModelBuilderConfigurations
    {
        public static ModelBuilder WithKiosk(this ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new KioskConfiguration())
                .ApplyConfiguration(new KioskSettingsConfiguration());
            return modelBuilder;
        }
    }
}
