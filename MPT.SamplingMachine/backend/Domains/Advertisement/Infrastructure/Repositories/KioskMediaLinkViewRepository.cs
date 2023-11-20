using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Advertisement.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Advertisement.Infrastructure.Repositories
{
    public class KioskMediaLinkViewRepository : Repository<KioskMediaLinkViewEntity, int>
    {
        public KioskMediaLinkViewRepository(AdvertisementDbContext context) : base(context) {
            _context = context;
        }

        public override IEnumerable<KioskMediaLinkViewEntity> Get(Func<KioskMediaLinkViewEntity, bool> predicate)
            => _context.KioskMediaLinkView.Include(x => x.Media).AsNoTracking().Where(predicate);

        public override void Put(IEnumerable<KioskMediaLinkViewEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly AdvertisementDbContext _context;
    }
}