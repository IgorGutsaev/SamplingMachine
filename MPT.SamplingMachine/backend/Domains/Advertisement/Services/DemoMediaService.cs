using Filuet.Infrastructure.Abstractions.Helpers;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Advertisement.Abstractions
{
    public class DemoMediaService : IMediaService
    {
        public IEnumerable<AdMedia> Get()
            => DemoData._media.Select(x=> { x.CanDelete = !DemoData._kiosks.SelectMany(k => k.Media).Select(m => m.Media.Hash).Distinct().Contains(x.Hash);
                return x;
            });

        public IEnumerable<KioskMediaLink> GetByKiosk(string kioskUid)
            => DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid)?.Media;

        public Dictionary<string, IEnumerable<KioskMediaLink>> GetByKiosks(IEnumerable<string> kiosks)
            => throw new NotImplementedException();

        public void Put(NewMediaRequest request) {
            if (DemoData._media.Any(x => x.Hash == request.Hash))
                return;

            DemoData._media.Add(new AdMedia {
                Hash = request.Hash,
                Name = Path.GetFileNameWithoutExtension(request.FileName),
                Type = EnumHelpers.GetValueFromCode<AdMediaType>(Path.GetExtension(request.FileName).Replace(".", ""))
            });
        }

        public void Delete(string hash)
            => DemoData._media.RemoveAll(x => x.Hash == hash);


    }
}
