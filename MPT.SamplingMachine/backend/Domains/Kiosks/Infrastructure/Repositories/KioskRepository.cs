using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Repositories
{
    public class KioskRepository : Repository<KioskEntity, int>
    {
        public KioskRepository(KioskDbContext context) : base (context) {
            _context = context;
        }

        public override IEnumerable<KioskEntity> Get(Func<KioskEntity, bool> predicate)
            => _context.Kiosks.Include(x => x.Settings).Include(x => x.Links).Where(predicate);

        public override void Put(IEnumerable<KioskEntity> entities)
            => throw new NotImplementedException();

        private readonly KioskDbContext _context;
    }
}