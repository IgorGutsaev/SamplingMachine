using Filuet.Hardware.Dispensers.Abstractions.Models;

namespace MPT.Vending.Domains.Kiosks.Abstractions
{
    public interface IReplenishmentService
    {
        event EventHandler<PoG> onPlanogramChanged;
        PoG GetPlanogram(string kioskUid);
        void PutPlanogram(string kioskUid, PoG planogram);
    }
}