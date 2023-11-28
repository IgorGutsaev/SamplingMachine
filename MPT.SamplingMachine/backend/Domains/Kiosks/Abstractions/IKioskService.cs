using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Abstractions.Events;

namespace MPT.Vending.Domains.Kiosks.Abstractions
{
    public interface IKioskService
    {
        event EventHandler<Kiosk> onKioskChanged;
        event EventHandler<PlanogramChangeEventArgs> onPlanogramChanged;

        Kiosk Get(string uid);
        void EnableDisable(string uid, bool enable);
        IEnumerable<Kiosk> GetAll();
        IEnumerable<Kiosk> Get(Func<Kiosk, bool> predicate);
        Kiosk Add(string uid);
        void AddOrUpdate(Kiosk kiosk);
        void SetCredit(string kioskUid, string sku, int credit);
        void SetMaxCountPerTransaction(string kioskUid, string sku, int limit);
        void SetMedia(string kioskUid, IEnumerable<KioskMediaLink> links);
        IEnumerable<Kiosk> GetKiosksWithSku(string sku);
        void Dispense(string kioskUid, string address);
    }
}