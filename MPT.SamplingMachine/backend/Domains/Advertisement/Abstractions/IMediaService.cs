using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Advertisement.Abstractions
{
    public interface IMediaService
    {
        IEnumerable<AdMedia> Get();
        AdMedia Get(string hash);
        IEnumerable<KioskMediaLink> GetByKiosk(string kioskUid);
        Dictionary<string, IEnumerable<KioskMediaLink>> GetByKiosks(IEnumerable<string> kiosks);
        void Put(NewMediaRequest request);
        void Delete(string hash);
    }
}