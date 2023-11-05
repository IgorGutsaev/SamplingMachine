using Filuet.Infrastructure.DataProvider;
using Filuet.Infrastructure.DataProvider.Interfaces;
using MPT.Vending.Domains.Products.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;
using MPT.Vending.Domains.SharedContext.Abstractions;
using MPT.Vending.Domains.SharedContext.Models;

namespace MPT.Vending.Domains.Products.Infrastructure.Repositories
{
    public class PictureRepository : Repository<PictureEntity, int>
    {
        const string PICTURES_CACHE_NAME = "ssoProfiles";
        const int PICTURES_CACHE_SIZE_MB = 200;
        const int PICTURES_CACHE_DURATION_MIN = 43200; // 30 days

        public PictureRepository(CatalogDbContext context,
            IMemoryCachingService memCache,
            IBlobRepository blobRepository) : base(context) {
            _context = context;
            _memCache = memCache;
            _blobRepository = blobRepository;
        }

        public override IEnumerable<PictureEntity> Get(Func<PictureEntity, bool> predicate)
            => _context.Pictures.Where(predicate);

        public override void Put(IEnumerable<PictureEntity> entities) {
            throw new NotImplementedException();
        }

        public async Task<string> GetPictureAsBase64(Guid uid) {
            MemoryCacher cacher = _memCache.Get(PICTURES_CACHE_NAME, PICTURES_CACHE_SIZE_MB);
            string data = cacher.Get<string>(uid);

            if (string.IsNullOrWhiteSpace(data)) {
                Blob blob = await _blobRepository.DownloadAsync($"products/pictures/{uid}");
                data = Convert.ToBase64String(blob.Data);
            }

            return data;
        }

        private readonly CatalogDbContext _context;
        private readonly IMemoryCachingService _memCache;
        private readonly IBlobRepository _blobRepository;
    }
}