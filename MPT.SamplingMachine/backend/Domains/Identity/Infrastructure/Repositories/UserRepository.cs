using MPT.Vending.Domains.Identity.Infrastructure.Entities;
using MPT.Vending.Domains.Kiosks.Infrastructure;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Identity.Infrastructure.Repositories
{
    public class UserRepository : Repository<UserEntity, Guid>
    {
        public UserRepository(IdentityDbContext context) : base(context) {
            _context = context;
        }

        public override IEnumerable<UserEntity> Get(Func<UserEntity, bool> predicate)
            => _context.Users.Where(predicate);

        public void Put(UserEntity user) {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public override void Put(IEnumerable<UserEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly IdentityDbContext _context;
    }
}