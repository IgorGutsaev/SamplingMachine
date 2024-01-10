using Filuet.Hardware.Dispensers.Abstractions.Models;
using MPT.Vending.Domains.Kiosks.Abstractions.Events;

namespace MPT.Vending.Domains.Kiosks.Abstractions
{
    public interface IReplenishmentService
    {
        event EventHandler<PlanogramChangeEventArgs> onPlanogramChanged;
        PoG GetPlanogram(string kioskUid);
        Dictionary<string, PoG> GetPlanograms();
        void PutPlanogram(string kioskUid, PoG planogram);
    }
}