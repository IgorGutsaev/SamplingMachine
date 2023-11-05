using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Entities
{
    public class KioskEntity : Entity<int>
    {
        public string Uid { get; set; }
        public bool IsOn { get; set; } = false;
        public IEnumerable<KioskSettingsEntity> Settings { get; set; }
    }
}