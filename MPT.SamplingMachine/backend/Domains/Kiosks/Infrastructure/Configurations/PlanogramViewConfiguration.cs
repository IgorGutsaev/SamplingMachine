using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Configurations
{
    public class PlanogramViewConfiguration : IEntityTypeConfiguration<PlanogramViewEntity> {
        public void Configure(EntityTypeBuilder<PlanogramViewEntity> builder) {
            builder.ToTable("PlanogramView");
            builder.HasNoKey();
        }
    }
}