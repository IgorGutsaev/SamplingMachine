using Filuet.Hardware.Dispensers.Abstractions.Models;

namespace MPT.Vending.Domains.Kiosks.Abstractions
{
    public interface IReplenishmentService
    {
        event EventHandler<PoG> onPlanogramChanged;
        PoG GetPlanogram(string kioskUid);
        Dictionary<string, PoG> GetPlanograms();
        void PutPlanogram(string kioskUid, PoG planogram);
    }
}