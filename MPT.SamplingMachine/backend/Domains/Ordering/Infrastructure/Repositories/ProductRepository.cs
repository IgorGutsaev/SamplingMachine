using MPT.Vending.Domains.Ordering.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Repositories
{
    public class ProductRepository : Repository<ProductEntity, int>
    {
        public ProductRepository(CatalogDbContext context) : base (context) {
            _context = context;
        }

        public override IEnumerable<ProductEntity> Get(Func<ProductEntity, bool> predicate)
            => _context.Products.Where(predicate);

        public override void Put(IEnumerable<ProductEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly CatalogDbContext _context;
    }
}