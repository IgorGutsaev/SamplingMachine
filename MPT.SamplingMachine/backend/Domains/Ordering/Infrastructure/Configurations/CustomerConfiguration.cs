using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPT.Vending.Domains.Ordering.Infrastructure.Entities;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<CustomerEntity>
    {
        public void Configure(EntityTypeBuilder<CustomerEntity> builder) {
            builder.ToTable("Customer");
            builder.HasKey(x => x.Id);
        }
    }
}