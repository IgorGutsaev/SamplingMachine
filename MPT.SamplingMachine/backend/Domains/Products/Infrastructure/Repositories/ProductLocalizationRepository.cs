using MPT.Vending.Domains.Products.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Products.Infrastructure.Repositories
{
    public class ProductLocalizationRepository : Repository<ProductLocalizationEntity, int>
    {
        public ProductLocalizationRepository(CatalogDbContext context) : base (context) {
            _context = context;
        }

        public override IEnumerable<ProductLocalizationEntity> Get(Func<ProductLocalizationEntity, bool> predicate)
            => _context.ProductLocalizations.Where(predicate);

        public override void Put(IEnumerable<ProductLocalizationEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly CatalogDbContext _context;
    }
}