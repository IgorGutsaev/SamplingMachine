using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Configurations
{
    public class PlanogramConfiguration : IEntityTypeConfiguration<PlanogramEntity> {
        public void Configure(EntityTypeBuilder<PlanogramEntity> builder) {
            builder.ToTable("Planogram");
            builder.HasKey(x => x.Id);
        }
    }
}