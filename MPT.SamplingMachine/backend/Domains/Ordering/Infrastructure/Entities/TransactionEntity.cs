using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Entities
{
    public class TransactionEntity : Entity<int>
    {
        public int CustomerId { get; set; }
        public CustomerEntity Customer { get; set; }
        public DateTimeOffset Date { get; set; }
        public ICollection<TransactionItemEntity> Items { get; set; }
    }
}