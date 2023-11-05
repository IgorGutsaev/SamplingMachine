using MPT.Vending.Domains.Products.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Products.Infrastructure.Repositories
{
    public class KioskProductLinkRepository : Repository<KioskProductLinkEntity, int>
    {
        public KioskProductLinkRepository(CatalogDbContext context) : base (context) {
            _context = context;
        }

        public override IEnumerable<KioskProductLinkEntity> Get(Func<KioskProductLinkEntity, bool> predicate)
            => _context.KioskProductLinks.Where(predicate);

        public override void Put(IEnumerable<KioskProductLinkEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly CatalogDbContext _context;
    }
}