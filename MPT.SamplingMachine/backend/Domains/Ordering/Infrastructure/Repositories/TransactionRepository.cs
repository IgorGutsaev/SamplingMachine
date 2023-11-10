using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Ordering.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Repositories
{
    public class TransactionRepository : Repository<TransactionEntity, int>
    {
        public TransactionRepository(CatalogDbContext context) : base (context) {
            _context = context;
        }

        public override IEnumerable<TransactionEntity> Get(Func<TransactionEntity, bool> predicate)
            => _context.Transactions.Include(x => x.Customer).Include(x => x.Items).ThenInclude(x => x.Product).Where(predicate);

        public override void Put(IEnumerable<TransactionEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly CatalogDbContext _context;
    }
}