using MPT.Vending.Domains.Products.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Products.Infrastructure.Repositories
{
    public class KioskProductLinkViewRepository : Repository<KioskProductLinkViewEntity, int>
    {
        public KioskProductLinkViewRepository(CatalogDbContext context) : base (context) {
            _context = context;
        }

        public override IEnumerable<KioskProductLinkViewEntity> Get(Func<KioskProductLinkViewEntity, bool> predicate)
            => _context.KioskProductLinksView.Where(predicate);

        public override void Put(IEnumerable<KioskProductLinkViewEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly CatalogDbContext _context;
    }
}