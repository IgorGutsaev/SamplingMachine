using MPT.Vending.Domains.Identity.Infrastructure.Entities;
using MPT.Vending.Domains.Kiosks.Infrastructure;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Identity.Infrastructure.Repositories
{
    public class ClaimRepository : Repository<ClaimEntity, int>
    {
        public ClaimRepository(IdentityDbContext context) : base(context) {
            _context = context;
        }

        public override IEnumerable<ClaimEntity> Get(Func<ClaimEntity, bool> predicate)
            => _context.Claims.Where(predicate);

        public override void Put(IEnumerable<ClaimEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly IdentityDbContext _context;
    }
}