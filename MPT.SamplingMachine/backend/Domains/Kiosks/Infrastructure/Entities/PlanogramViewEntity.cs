
namespace MPT.Vending.Domains.Kiosks.Infrastructure.Entities
{
    public class PlanogramViewEntity
    {
        public int KioskId { get; set; }
        public string KioskUid { get; set; }
        public string Planogram { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}