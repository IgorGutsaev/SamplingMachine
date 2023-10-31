using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Advertisement.Abstractions
{
    public interface IMediaService
    {
        IEnumerable<AdMedia> Get();
        IEnumerable<KioskMediaLink> GetByKiosk(string kioskUid);
        void Put(NewMediaRequest request);
        void Delete(string hash);
    }
}