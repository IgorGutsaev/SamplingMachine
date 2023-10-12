using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Advertisement.Abstractions
{
    public interface IMediaService
    {
        IEnumerable<KioskMediaLink> Get(string kioskUid);
        void Put(Guid uid, AdMediaType media, string name);
    }
}
