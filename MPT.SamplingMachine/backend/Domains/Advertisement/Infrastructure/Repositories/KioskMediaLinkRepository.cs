using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Advertisement.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Advertisement.Infrastructure.Repositories
{
    public class KioskMediaLinkRepository : Repository<KioskMediaLinkEntity, int>
    {
        public KioskMediaLinkRepository(AdvertisementDbContext context) : base(context) {
            _context = context;
        }

        public override IEnumerable<KioskMediaLinkEntity> Get(Func<KioskMediaLinkEntity, bool> predicate)
            => _context.KioskMediaLinks.Include(x => x.Media).Where(predicate);

        public override void Put(IEnumerable<KioskMediaLinkEntity> entities) {
            throw new NotImplementedException();
        }

        private readonly AdvertisementDbContext _context;
    }
}