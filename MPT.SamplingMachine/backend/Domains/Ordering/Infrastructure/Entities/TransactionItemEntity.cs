using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Entities
{
    public class TransactionItemEntity : Entity<int>
    {
        public int Count { get; set; }
        public int UnitCredit { get; set; }
        public int TransactionId { get; set; }
        public TransactionEntity Transaction { get; set; }
        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }
    }
}