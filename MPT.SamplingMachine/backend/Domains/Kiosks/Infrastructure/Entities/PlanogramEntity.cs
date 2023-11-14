using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Entities
{
    public class PlanogramEntity : Entity<int>
    {
        public int KioskId { get; set; }
        public string Planogram { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}