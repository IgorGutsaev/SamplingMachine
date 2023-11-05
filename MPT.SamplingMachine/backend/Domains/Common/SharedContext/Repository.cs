using Microsoft.EntityFrameworkCore;

namespace MPT.Vending.Domains.SharedContext
{
    public abstract class Repository<T, T1> where T : Entity<T1>
    {
        public Repository(DbContext context) {
            _context = context;
        }

        public abstract IEnumerable<T> Get(Func<T, bool> predicate);

        public virtual T Put(T entity) {
            if (typeof(T1) == typeof(int)) {
                if (Convert.ToInt32(entity.Id) > 0) {
                    _context.Update(entity);
                    _context.SaveChanges();
                    return entity;
                }
                else {
                    var e = _context.Add(entity);
                    _context.SaveChanges();
                    return e.Entity;
                }
            }
            else throw new NotImplementedException();
        }

        public abstract void Put(IEnumerable<T> entities);

        private readonly DbContext _context;
    }
}
