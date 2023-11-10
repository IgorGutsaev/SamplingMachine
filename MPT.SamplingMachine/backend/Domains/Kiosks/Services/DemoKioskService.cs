using Filuet.Infrastructure.Abstractions.Enums;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Services
{
    public class DemoKioskService : IKioskService {
        public event EventHandler<Kiosk> onKioskChanged;

        public Kiosk Get(string uid) {
            if (DemoData._kiosks.Any(x => x.UID == uid))
                return DemoData._kiosks.First(x => x.UID == uid);

            return null;
        }

        public void EnableDisable(string uid, bool enabled) {
            if (DemoData._kiosks.Any(x => x.UID == uid)) {
                Kiosk kiosk = DemoData._kiosks.First(x => x.UID == uid);
                if (kiosk.IsOn != enabled) {
                    kiosk.IsOn = enabled;
                    onKioskChanged?.Invoke(this, kiosk);
                }
            }
        }

        public IEnumerable<Kiosk> GetAll()
            => DemoData._kiosks;

        public IEnumerable<Kiosk> Get(Func<Kiosk, bool> predicate)
            => DemoData._kiosks.Where(predicate);

        public Kiosk Add(string uid) {
            if (string.IsNullOrWhiteSpace(uid))
                throw new ArgumentException("Kiosk uid is mandatory");

            if (DemoData._kiosks.Any(x => x.UID == uid))
                throw new ArgumentException("A kiosk with the same name already exists");

            Kiosk result = new Kiosk {
                UID = uid,
                Credit = 1,
                IdleTimeout = TimeSpan.FromMinutes(3),
                Languages = new Language[] { Language.English }
            };

            DemoData._kiosks.Insert(0, result);

            return result;
        }

        public void AddOrUpdate(Kiosk kiosk) {
            if (kiosk == null)
                throw new NullReferenceException();

            if (string.IsNullOrWhiteSpace(kiosk.UID))
                throw new ArgumentException("Kiosk uid is mandatory");

            Kiosk existedKiosk = DemoData._kiosks.FirstOrDefault(x => x.UID == kiosk.UID);

            bool isNewKiosk = false;
            if (existedKiosk == null) {
                existedKiosk = Add(kiosk.UID);
                isNewKiosk = true;
            }

            existedKiosk.Merge(kiosk);
            if (!isNewKiosk)
                onKioskChanged?.Invoke(this, kiosk);
        }

        public void SetCredit(string kioskUid, string sku, int credit) {
            bool changed = false;
            Kiosk kiosk = DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid);

            if (string.IsNullOrWhiteSpace(sku)) {
                if (kiosk.Credit != credit) {
                    kiosk.Credit = credit;
                    changed = true;
                }
            }
            else {
                KioskProductLink link = kiosk.ProductLinks.FirstOrDefault(x => x.Product.Sku == sku);
                if (link != null && link.Credit != credit) {
                    link.Credit = credit;
                    changed = true;
                }
            }

            if (changed)
                onKioskChanged?.Invoke(this, kiosk);
        }

        public void SetMaxCountPerTransaction(string kioskUid, string sku, int limit) {
            KioskProductLink link = DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks.FirstOrDefault(x => x.Product.Sku == sku);
            if (link != null)
                link.MaxCountPerTransaction = limit;
        }

        public void SetMedia(string kioskUid, IEnumerable<KioskMediaLink> links) {
            Kiosk kiosk = DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid);
            kiosk.Media = links;
            onKioskChanged?.Invoke(this, kiosk);
        }

        public IEnumerable<Kiosk> GetKiosksWithSku(string sku)
            => DemoData._kiosks.Where(x => x.ProductLinks.Any(l => l.Product.Sku == sku));
    }
}