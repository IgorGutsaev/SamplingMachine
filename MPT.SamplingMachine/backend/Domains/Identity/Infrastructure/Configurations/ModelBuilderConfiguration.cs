using Microsoft.EntityFrameworkCore;

namespace MPT.Vending.Domains.Identity.Infrastructure.Configurations
{
    public static class ModelBuilderConfigurations
    {
        public static ModelBuilder WithIdentity(this ModelBuilder modelBuilder) {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            return modelBuilder;
        }
    }
}