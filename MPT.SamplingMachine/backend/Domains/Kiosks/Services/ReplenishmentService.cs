using Filuet.Hardware.Dispensers.Abstractions.Models;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.Kiosks.Abstractions.Events;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;
using MPT.Vending.Domains.Kiosks.Infrastructure.Repositories;

namespace MPT.Vending.Domains.Kiosks.Services
{
    public class ReplenishmentService : IReplenishmentService
    {
        public event EventHandler<PlanogramChangeEventArgs> onPlanogramChanged;

        public ReplenishmentService(PlanogramRepository planogramRepository, PlanogramViewRepository planogramViewRepository) {
            _planogramRepository = planogramRepository;
            _planogramViewRepository = planogramViewRepository;
        }

        public PoG GetPlanogram(string kioskUid) {
            PlanogramViewEntity? planogram = _planogramViewRepository.Get(x => x.KioskUid == kioskUid).FirstOrDefault();

            if (planogram == null)
                throw new Exception("Planogram missing");

            return PoG.Read(planogram.Planogram);
        }

        public Dictionary<string, PoG> GetPlanograms()
            => _planogramViewRepository.Get(x => true)
            .Select(x => new KeyValuePair<string, PoG>(x.KioskUid, PoG.Read(x.Planogram)))
            .ToDictionary(x => x.Key, x => x.Value);

        public void PutPlanogram(string kioskUid, PoG planogram) {
            PlanogramViewEntity? viewEntity = _planogramViewRepository.Get(x => x.KioskUid == kioskUid).FirstOrDefault();
            if (viewEntity == null)
                throw new Exception("Planogram not found");

            PlanogramEntity planogramEntity = _planogramRepository.Get(x => x.KioskId == viewEntity.KioskId).FirstOrDefault();
            planogramEntity.Planogram = planogram.ToString(false);
            _planogramRepository.Put(planogramEntity);

            onPlanogramChanged?.Invoke(this, new PlanogramChangeEventArgs { KioskUid = kioskUid, Planogram = planogram });
        }

        private readonly PlanogramRepository _planogramRepository;
        private readonly PlanogramViewRepository _planogramViewRepository;
    }
}
