using MPT.Vending.Domains.Identity.Infrastructure.Entities;
using MPT.Vending.Domains.Kiosks.Infrastructure;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Identity.Infrastructure.Repositories
{
    public class UserClaimRepository : Repository<UserClaimEntity, int>
    {
        public UserClaimRepository(IdentityDbContext context) : base(context) {
            _context = context;
        }

        public override IEnumerable<UserClaimEntity> Get(Func<UserClaimEntity, bool> predicate)
            => _context.UserClaims.Where(predicate);

        public override void Put(IEnumerable<UserClaimEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly IdentityDbContext _context;
    }
}