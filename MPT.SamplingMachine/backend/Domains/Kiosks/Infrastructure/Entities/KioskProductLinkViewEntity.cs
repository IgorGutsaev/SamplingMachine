using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Entities
{
    public class KioskProductLinkViewEntity : Entity<int>
    {
        public string KioskUid { get; set; }
        public string Sku { get; set; }
        public int MaxCountPerTransaction { get; set; }
        public int Credit { get; set; }
        public bool Disabled { get; set; }
    }
}