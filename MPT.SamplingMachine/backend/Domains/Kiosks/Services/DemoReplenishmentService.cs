using Filuet.Hardware.Dispensers.Abstractions.Models;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.Kiosks.Abstractions.Events;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Services
{
    public class DemoReplenishmentService : IReplenishmentService
    {
        public event EventHandler<PlanogramChangeEventArgs> onPlanogramChanged;
        public PoG GetPlanogram(string kioskUid)
            => DemoData._planogram;

        public Dictionary<string, PoG> GetPlanograms() => throw new NotImplementedException();
        public void PutPlanogram(string kioskUid, PoG planogram) {
            DemoData._planogram = planogram;
        }
    }
}