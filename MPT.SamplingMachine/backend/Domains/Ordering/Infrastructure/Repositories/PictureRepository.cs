using Filuet.Infrastructure.DataProvider;
using Filuet.Infrastructure.DataProvider.Interfaces;
using MPT.Vending.Domains.Ordering.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;
using MPT.Vending.Domains.SharedContext.Abstractions;
using MPT.Vending.Domains.SharedContext.Models;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Repositories
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

        public async Task<string> GetPictureAsBase64Async(int pictureId) {
            PictureEntity? picture = Get(x => x.Id == pictureId).FirstOrDefault();
            if (picture == null)
                return string.Empty;

            return await GetPictureAsBase64Async(picture.Uid);
        }

        public async Task<string> GetPictureAsBase64Async(Guid uid) {
            MemoryCacher cacher = _memCache.Get(PICTURES_CACHE_NAME, PICTURES_CACHE_SIZE_MB);
            string data = cacher.Get<string>(uid);

            if (string.IsNullOrWhiteSpace(data)) {
                Blob blob = null;
                try {
                    blob = await _blobRepository.DownloadAsync($"products/pictures/{uid}");
                }
                catch (ArgumentException) { return string.Empty; }
                data = Convert.ToBase64String(blob.Data);
                cacher.Set(uid, data);
            }

            return data;
        }

        /// <summary>
        /// Store picture in a cloud storage
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="picture"></param>
        /// <returns></returns>
        public async Task<PictureEntity?> PutPictureAsBase64Async(int? pictureId, byte[] picture) {
            bool createAnEntity = false;
            PictureEntity? result = null;
            Guid uid = Guid.Empty;

            if (pictureId.HasValue) {
                PictureEntity? pEntity = Get(x => x.Id == pictureId.Value).FirstOrDefault();
                if (pEntity != null) {
                    string stored = string.Empty;
                    try {
                        stored = await GetPictureAsBase64Async(pEntity.Uid);
                    }
                    catch (ArgumentException) {
                        stored = string.Empty;
                    }
                    
                    if (stored != Convert.ToBase64String(picture)) { // a new picture 
                        uid = Guid.NewGuid();
                        await _blobRepository.UploadAsync(picture, $"products/pictures/{uid}");
                    }
                    result = pEntity;
                }
                else createAnEntity = true;
            }
            else createAnEntity = true;

            if (createAnEntity) {
                uid = Guid.NewGuid();
                PictureEntity e = new PictureEntity { Uid = uid };
                _context.Pictures.Add(e);
                _context.SaveChanges();
                pictureId = e.Id;
                await _blobRepository.UploadAsync(picture, $"products/pictures/{uid}");
                result = e;
            }

            MemoryCacher cacher = _memCache.Get(PICTURES_CACHE_NAME, PICTURES_CACHE_SIZE_MB);
            cacher.Set(uid, Convert.ToBase64String(picture));

            return result;
        }

        private readonly CatalogDbContext _context;
        private readonly IMemoryCachingService _memCache;
        private readonly IBlobRepository _blobRepository;
    }
}