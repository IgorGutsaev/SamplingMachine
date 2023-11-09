using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Repositories
{
    public class KioskProductLinkViewRepository : Repository<KioskProductLinkViewEntity, int>
    {
        public KioskProductLinkViewRepository(KioskDbContext context) : base (context) {
            _context = context;
        }

        public override IEnumerable<KioskProductLinkViewEntity> Get(Func<KioskProductLinkViewEntity, bool> predicate)
            => _context.KioskProductLinksView.Where(predicate);

        public override void Put(IEnumerable<KioskProductLinkViewEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly KioskDbContext _context;
    }
}