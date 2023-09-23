using Filuet.Infrastructure.Abstractions.Enums;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Services
{
    public class DemoKioskService : IKioskService
    {
        public KioskDto Get(string uid)
        {
            if (DemoData._kiosks.Any(x => x.UID == uid))
                return DemoData._kiosks.First(x => x.UID == uid);

            return null;
        }

        public void EnableDisable(string uid, bool enabled)
        {
            if (DemoData._kiosks.Any(x => x.UID == uid))
                DemoData._kiosks.First(x => x.UID == uid).IsOn = enabled;
        }

        public IEnumerable<KioskDto> GetAll()
            => DemoData._kiosks;

        public KioskDto Add(string uid)
        {
            if (string.IsNullOrWhiteSpace(uid))
                throw new ArgumentException("Kiosk uid is mandatory");

            if (DemoData._kiosks.Any(x => x.UID == uid))
                throw new ArgumentException("A kiosk with the same name already exists");

            KioskDto result = new KioskDto {
                UID = uid, Credit= 1,
                IdleTimeout = TimeSpan.FromMinutes(3),
                Languages= new Language[] { Language.English}
            };

            DemoData._kiosks.Insert(0, result);

            return result;
        }

        public void AddOrUpdate(KioskDto kiosk)
        {
            if (kiosk == null)
                throw new NullReferenceException();

            if (string.IsNullOrWhiteSpace(kiosk.UID))
                throw new ArgumentException("Kiosk uid is mandatory");

            KioskDto existedKiosk = DemoData._kiosks.FirstOrDefault(x => x.UID == kiosk.UID);

            if (existedKiosk == null)
                existedKiosk = Add(kiosk.UID);

            existedKiosk.Merge(kiosk);
        }

        public void DisableProductLink(string kioskUid, string sku)
        {
            if (DemoData._kiosks.Any(x => x.UID == kioskUid))
                DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks.FirstOrDefault(x => x.Product.Sku == sku).Disabled = true;
        }

        public void EnableProductLink(string kioskUid, string sku)
        {
            if (DemoData._kiosks.Any(x => x.UID == kioskUid))
                DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks.FirstOrDefault(x => x.Product.Sku == sku).Disabled = false;
        }

        public void AddProductLink(string kioskUid, string sku)
        {
            DemoData.Link(kioskUid, sku);
        }

        public void DeleteProductLink(string kioskUid, string sku)
        {
            if (DemoData._kiosks.Any(x => x.UID == kioskUid) && DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks.Any(x => x.Product.Sku == sku))
                DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks =
                    DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks.Where(x => x.Product.Sku != sku);
        }

        public void SetCredit(string kioskUid, string sku, int credit)
        {
            KioskProductLink link = DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks.FirstOrDefault(x => x.Product.Sku == sku);
            if (link != null)
                link.Credit = credit;
        }

        public void SetMaxCountPerSession(string kioskUid, string sku, int limit)
        {
            KioskProductLink link = DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks.FirstOrDefault(x => x.Product.Sku == sku);
            if (link != null)
                link.MaxCountPerSession = limit;
        }
    }
}