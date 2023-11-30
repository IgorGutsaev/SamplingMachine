using MPT.Vending.Domains.SharedContext;
using System.ComponentModel.DataAnnotations.Schema;

namespace MPT.Vending.Domains.Identity.Infrastructure.Entities
{
    public class UserEntity : Entity<Guid>
    {
        public string Email { get; set; }
        [Column(TypeName = "nvarchar(250)")]
        public string Password { get; set; } = null;
        public bool Admin { get; set; }
    }
}
