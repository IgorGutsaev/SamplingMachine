using Filuet.Infrastructure.Abstractions.Helpers;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Advertisement.Abstractions
{
    public class DemoMediaService : IMediaService
    {
        public IEnumerable<AdMedia> Get()
            => DemoData._media;

        public IEnumerable<KioskMediaLink> GetByKiosk(string kioskUid)
            => DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid)?.Media;

        public void Put(NewMediaRequest request) {
            if (DemoData._media.Any(x => x.Hash == request.Hash))
                return;

            DemoData._media.Add(new AdMedia {
                Hash = request.Hash,
                Name = Path.GetFileNameWithoutExtension(request.FileName),
                Type = EnumHelpers.GetValueFromCode<AdMediaType>(Path.GetExtension(request.FileName).Replace(".", ""))
            });
        }
    }
}
