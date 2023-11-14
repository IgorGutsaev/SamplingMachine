using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Repositories
{
    public class PlanogramViewRepository
    {
        public PlanogramViewRepository(KioskDbContext context) {
            _context = context;
        }

        public IEnumerable<PlanogramViewEntity> Get(Func<PlanogramViewEntity, bool> predicate)
            => _context.PlanogramView.Where(predicate);

        private readonly KioskDbContext _context;
    }
}
