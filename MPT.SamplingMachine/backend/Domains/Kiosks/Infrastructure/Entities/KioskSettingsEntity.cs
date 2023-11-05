using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Entities
{
    public class KioskSettingsEntity : Entity<int>
    {
        public int KioskId { get; set; }
        public KioskEntity Kiosk { get; set; }
        public string Identifier { get; set; }
        public string Value { get; set; }

        public static KioskSettingsEntity Create(int kioskId, string identifier, object value) {
            if (kioskId <= 0)
                throw new ArgumentException("Kiosk is mandatory");

            if (string.IsNullOrWhiteSpace(identifier))
                throw new ArgumentException("Identifier is mandatory");

            if (value == null || value.ToString() == string.Empty)
                throw new ArgumentException("Value is mandatory");

            return new KioskSettingsEntity { KioskId = kioskId, Identifier = identifier, Value = value.ToString() };
        }
    }
}