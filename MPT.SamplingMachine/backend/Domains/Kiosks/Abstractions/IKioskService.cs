using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Kiosks.Abstractions
{
    public interface IKioskService
    {
        KioskDto Get(string uid);
        IEnumerable<KioskDto> GetAll();
        KioskDto Add(string uid);
        void AddOrUpdate(KioskDto kiosk);
        void DisableProductLink(string kioskUid, string sku);
        void EnableProductLink(string kioskUid, string sku);
        void AddProductLink(string kioskUid, string sku);
        void DeleteProductLink(string kioskUid, string sku);
        void SetCredit(string kioskUid, string sku, int credit);
        void SetMaxCountPerSession(string kioskUid, string sku, int limit);
    }
}
