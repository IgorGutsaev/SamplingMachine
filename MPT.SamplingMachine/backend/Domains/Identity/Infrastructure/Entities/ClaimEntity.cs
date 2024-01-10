using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Identity.Infrastructure.Entities
{
    public class ClaimEntity : Entity<int>
    {
        public string Name { get; set; }
    }
}
