using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Ordering.Infrastructure.Configurations;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Configurations
{
    public static class ModelBuilderConfigurations
    {
        public static ModelBuilder WithKiosk(this ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new KioskConfiguration())
                .ApplyConfiguration(new KioskSettingsConfiguration())
                .ApplyConfiguration(new PlanogramConfiguration())
                .ApplyConfiguration(new PlanogramViewConfiguration())
                .ApplyConfiguration(new KioskProductLinkViewConfiguration());
            return modelBuilder;
        }
    }
}