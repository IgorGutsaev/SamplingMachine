using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Repositories
{
    public class PlanogramRepository : Repository<PlanogramEntity, int>
    {
        public PlanogramRepository(KioskDbContext context) : base(context) {
            _context = context;
        }

        public override IEnumerable<PlanogramEntity> Get(Func<PlanogramEntity, bool> predicate)
            => _context.Planograms.Where(predicate);

        public void Put(PlanogramEntity planogram) {
            _context.Planograms.Update(planogram);
            _context.SaveChanges();
        }

        public override void Put(IEnumerable<PlanogramEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly KioskDbContext _context;
    }
}
