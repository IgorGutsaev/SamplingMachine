using MPT.Vending.Domains.Ordering.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<CustomerEntity, int>
    {
        public CustomerRepository(CatalogDbContext context) : base (context) {
            _context = context;
        }

        public override IEnumerable<CustomerEntity> Get(Func<CustomerEntity, bool> predicate)
            => _context.Customers.Where(predicate);

        public override CustomerEntity Put(CustomerEntity entity) {
            CustomerEntity result = _context.Customers.FirstOrDefault(x => x.PhoneNumber == entity.PhoneNumber);
            if (result != null)
                return result;

            _context.Customers.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public override void Put(IEnumerable<CustomerEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly CatalogDbContext _context;
    }
}