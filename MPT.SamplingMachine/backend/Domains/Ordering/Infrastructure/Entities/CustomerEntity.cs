using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Entities
{
    public class CustomerEntity : Entity<int>
    {
        public string PhoneNumber { get; set; }
    }
}