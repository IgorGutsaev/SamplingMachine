using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Identity.Infrastructure.Entities
{
    public class UserEntity : Entity<Guid>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Admin { get; set; }
    }
}
