using MPT.Vending.Domains.Advertisement.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Advertisement.Infrastructure.Repositories
{
    public class AdMediaRepository : Repository<AdMediaEntity, int>
    {
        public AdMediaRepository(AdvertisementDbContext context) : base(context) {
            _context = context;
        }

        public override IEnumerable<AdMediaEntity> Get(Func<AdMediaEntity, bool> predicate)
            => _context.Media.Where(predicate);

        public override void Put(IEnumerable<AdMediaEntity> entities) {
            throw new NotImplementedException();
        }

        public void Delete(string hash) {
            AdMediaEntity? entity = _context.Media.FirstOrDefault(x => x.Hash == hash.ToLower());
            if (entity == null)
                return;

            _context.Media.Remove(entity);
            _context.SaveChanges();
        }

        private readonly AdvertisementDbContext _context;
    }
}