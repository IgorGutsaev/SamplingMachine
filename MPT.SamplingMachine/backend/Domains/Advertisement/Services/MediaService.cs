using MPT.Vending.API.Dto;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Advertisement.Abstractions
{
    public class MediaService : IMediaService
    {
        public IEnumerable<KioskMediaLink> Get(string kioskUid)
            => DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid)?.Media;

        public void Put(Guid uid, AdMediaType media, string name)
        {
            throw new NotImplementedException();
        }
    }
}
