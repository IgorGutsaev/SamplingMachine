using Filuet.Infrastructure.Abstractions.Enums;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.Kiosks.Infrastructure.Builders;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;
using MPT.Vending.Domains.Kiosks.Infrastructure.Repositories;
using MPT.Vending.Domains.SharedContext;
using System.Text.Json;

namespace MPT.Vending.Domains.Kiosks.Services
{
    public class KioskService : IKioskService
    {
        public event EventHandler<Kiosk> onKioskChanged;

        public KioskService(KioskRepository kioskRepository,
            KioskSettingsRepository kioskSettingsRepository,
            KioskProductLinkViewRepository kioskProductLinkViewRepository,
            Func<IEnumerable<string>, IEnumerable<Product>> getProducts)
        {
            _kioskRepository = kioskRepository;
            _kioskSettingsRepository = kioskSettingsRepository;
            _kioskProductLinkViewRepository = kioskProductLinkViewRepository;
            _getProducts = getProducts;
        }

        public Kiosk Get(string uid) {
            uid = uid.ToUpper();

            KioskEntity kiosk = _kioskRepository.Get(x => x.Uid == uid).FirstOrDefault();
            if (kiosk == null)
                return null;

            IEnumerable<KioskSettingsEntity> settings = _kioskSettingsRepository.Get(x => x.KioskId == kiosk.Id).ToList();
            IEnumerable<KioskProductLinkViewEntity> links = _kioskProductLinkViewRepository.Get(x => x.KioskUid == uid).ToList();
            IEnumerable<Product> products = _getProducts(links.Select(x => x.Sku));

            Kiosk result = new KioskBuilder()
                .WithData(kiosk)
                .WithSettings(settings)
                .WithLinks(links, products)
                .Build();

            return result;
        }

        public void EnableDisable(string uid, bool enabled) {
            uid = uid.ToUpper();

            KioskEntity kiosk = _kioskRepository.Get(x => x.Uid == uid).First();

            if (kiosk.IsOn != enabled) {
                kiosk.IsOn = enabled;
                onKioskChanged?.Invoke(this, Get(uid)); // fire an event first
                _kioskRepository.Put(kiosk); // save changes
            }
        }

        public IEnumerable<Kiosk> GetAll() {
            IEnumerable<KioskEntity> kiosks = _kioskRepository.Get(x => true).ToList();
            IEnumerable<KioskSettingsEntity> settings = _kioskSettingsRepository.Get(x => kiosks.Select(k => k.Id).Contains(x.KioskId)).ToList();
            IEnumerable<KioskProductLinkViewEntity> links = _kioskProductLinkViewRepository.Get(x => kiosks.Select(k => k.Uid).Contains(x.KioskUid)).ToList();
            IEnumerable<Product> products = _getProducts(links.Select(x => x.Sku));

            foreach (var k in kiosks) {
                yield return new KioskBuilder()
                    .WithData(k)
                    .WithSettings(settings.Where(x => x.KioskId == k.Id))
                    .WithLinks(links.Where(x => x.KioskUid == k.Uid), products)
                    .Build();
            }
        }

        public IEnumerable<Kiosk> Get(Func<Kiosk, bool> predicate)
            => GetAll().Where(predicate);

        public Kiosk Add(string uid) {
            if (string.IsNullOrWhiteSpace(uid))
                throw new ArgumentException("Kiosk uid is mandatory");

            uid = uid.Trim().ToUpper();

            if (Get(uid) != null)
                throw new ArgumentException("A kiosk with the same name already exists");

            KioskEntity kiosk = _kioskRepository.Put(new KioskEntity { Uid = uid, IsOn = false });
            _kioskSettingsRepository.Put(new KioskSettingsEntity[] {
                KioskSettingsEntity.Create(kiosk.Id, "Credit", 1),
                KioskSettingsEntity.Create(kiosk.Id, "IdleTimeout", TimeSpan.FromMinutes(3)),
                KioskSettingsEntity.Create(kiosk.Id, "Languages", JsonSerializer.Serialize(new Language[] { Language.English }))
            });

            return new Kiosk {
                IsOn = false,
                UID = uid,
                Credit = 1,
                IdleTimeout = TimeSpan.FromMinutes(3),
                Languages = new Language[] { Language.English }
            };
        }

        public void AddOrUpdate(Kiosk kiosk) {
            if (kiosk == null)
                throw new NullReferenceException();

            if (string.IsNullOrWhiteSpace(kiosk.UID))
                throw new ArgumentException("Kiosk uid is mandatory");

            Kiosk existedKiosk = Get(kiosk.UID);

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
            => Get(x => _kioskProductLinkViewRepository.Get(x => x.Sku == sku).Select(x => x.KioskUid).ToList().Distinct().Contains(x.UID));

        private readonly KioskRepository _kioskRepository;
        private readonly KioskSettingsRepository _kioskSettingsRepository;
        private readonly KioskProductLinkViewRepository _kioskProductLinkViewRepository;
        private readonly Func<IEnumerable<string>, IEnumerable<Product>> _getProducts;
    }
}