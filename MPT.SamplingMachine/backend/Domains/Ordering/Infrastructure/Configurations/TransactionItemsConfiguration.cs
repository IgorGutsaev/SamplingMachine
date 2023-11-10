using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MPT.Vending.Domains.Ordering.Infrastructure.Entities;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Configurations
{
    public class TransactionItemsConfiguration : IEntityTypeConfiguration<TransactionItemEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionItemEntity> builder) {
            builder.ToTable("TxnItem");
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Transaction);
            builder.HasOne(x => x.Product);
        }
    }
}