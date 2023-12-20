using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Identity.Infrastructure.Entities
{
    public class UserClaimEntity : Entity<int>
    {
        public int ClaimId { get; set; }
        public ClaimEntity Claim { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }
}