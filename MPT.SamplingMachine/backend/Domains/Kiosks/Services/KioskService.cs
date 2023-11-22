using Filuet.Hardware.Dispensers.Abstractions.Models;
using Filuet.Infrastructure.Abstractions.Enums;
using Microsoft.Extensions.Caching.Distributed;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Abstractions;
using MPT.Vending.Domains.Kiosks.Infrastructure.Builders;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;
using MPT.Vending.Domains.Kiosks.Infrastructure.Repositories;
using MPT.Vending.Domains.SharedContext;
using System.Diagnostics;
using System.Text.Json;

namespace MPT.Vending.Domains.Kiosks.Services
{
    public class KioskService : IKioskService
    {
        public event EventHandler<Kiosk> onKioskChanged;

        public KioskService(KioskRepository kioskRepository,
            KioskSettingsRepository kioskSettingsRepository,
            KioskProductLinkViewRepository kioskProductLinkViewRepository,
            PlanogramRepository planogramRepository,
            PlanogramViewRepository planogramViewRepository,
            Func<IEnumerable<string>, IEnumerable<Product>> getProducts,
            Func<IEnumerable<string>, Dictionary<string, IEnumerable<KioskMediaLink>>> getMedia) {
            _kioskRepository = kioskRepository;
            _kioskSettingsRepository = kioskSettingsRepository;
            _kioskProductLinkViewRepository = kioskProductLinkViewRepository;
            _planogramRepository = planogramRepository;
            _planogramViewRepository = planogramViewRepository;
            _getProducts = getProducts;
            _getMedia = getMedia;
        }

        public Kiosk Get(string uid) {
            uid = uid.ToUpper();

            KioskEntity kiosk = _kioskRepository.Get(x => x.Uid == uid).FirstOrDefault();
            if (kiosk == null)
                return null;

            IEnumerable<Product> products = _getProducts(kiosk.Links.Select(x => x.Sku));
            Dictionary<string, IEnumerable<KioskMediaLink>> media = _getMedia(new string[] { uid });
            PlanogramViewEntity? planogram = _planogramViewRepository.Get(x => x.KioskUid == uid).FirstOrDefault();

            Kiosk result = new KioskBuilder()
                .WithData(kiosk, products)
                .WithMedia(media.TryGetValue(uid, out IEnumerable<KioskMediaLink>? value) ? value : null)
                .WithPlanogram(planogram == null ? null : PoG.Read(planogram.Planogram))
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
            IEnumerable<Product> products = _getProducts(kiosks.SelectMany(x => x.Links).Select(x => x.Sku).Distinct()).ToList();
            Dictionary<string, IEnumerable<KioskMediaLink>> media = _getMedia(kiosks.Select(x => x.Uid));
            IEnumerable<PlanogramViewEntity> planograms = _planogramViewRepository.Get(x => true);

            foreach (var k in kiosks) {
                yield return new KioskBuilder()
                    .WithData(k, products)
                    .WithMedia(media.TryGetValue(k.Uid, out IEnumerable<KioskMediaLink>? value) ? value : null)
                    .WithPlanogram(PoG.Read(planograms.FirstOrDefault(x => x.KioskUid == k.Uid)?.Planogram))
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
            else {
                KioskEntity kioskEntity = _kioskRepository.Get(x => x.Uid == kiosk.UID).First();
                IEnumerable<KioskSettingsEntity> settings = _kioskSettingsRepository.Get(x => x.KioskId == kioskEntity.Id).ToList();
                List<KioskSettingsEntity> toAdd = new List<KioskSettingsEntity>();
   
                KioskSettingsEntity? idleTimeout = settings.FirstOrDefault(x => x.Identifier == "IdleTimeout");
                if (idleTimeout == null)
                    toAdd.Add(KioskSettingsEntity.Create(kioskEntity.Id, "IdleTimeout", kiosk.IdleTimeout.ToString()));
                else if (idleTimeout.Value != kiosk.IdleTimeout.ToString()) {
                    idleTimeout.Value = kiosk.IdleTimeout.ToString();
                    _kioskSettingsRepository.Put(idleTimeout);
                }

                KioskSettingsEntity? languages = settings.FirstOrDefault(x => x.Identifier == "Languages");
                if (languages == null)
                    toAdd.Add(KioskSettingsEntity.Create(kioskEntity.Id, "Languages", JsonSerializer.Serialize(kiosk.Languages)));
                else if (languages.Value != JsonSerializer.Serialize(kiosk.Languages)) {
                    languages.Value = JsonSerializer.Serialize(kiosk.Languages);
                    _kioskSettingsRepository.Put(languages);
                }

                if (toAdd.Any())
                    _kioskSettingsRepository.Put(toAdd);
            }

            existedKiosk.Merge(kiosk);
            if (!isNewKiosk)
                onKioskChanged?.Invoke(this, kiosk);
        }

        public void SetCredit(string kioskUid, string sku, int credit) {
            bool changed = false;
            kioskUid = kioskUid.ToUpper();
            KioskEntity kiosk = _kioskRepository.Get(x => x.Uid == kioskUid).First();

            if (string.IsNullOrWhiteSpace(sku)) {
                KioskSettingsEntity? creditEntity = kiosk.Settings.FirstOrDefault(x => x.Identifier == "Credit");
                if (creditEntity == null) {
                    _kioskSettingsRepository.Put(KioskSettingsEntity.Create(kiosk.Id, "Credit", credit));
                    changed = true;
                }
                else if (creditEntity.Value != credit.ToString()) {
                    creditEntity.Value = credit.ToString();
                    creditEntity.Kiosk = null;
                    _kioskSettingsRepository.Put(creditEntity);
                    changed = true;
                }
            }
            else {
                KioskProductLinkViewEntity? link = kiosk.Links.FirstOrDefault(x => x.Sku == sku);
                if (link != null && link.Credit != credit) {
                    _kioskProductLinkViewRepository.SetCredit(credit, link.Kiosk.Id, link.Sku);
                    changed = true;
                }
            }

            if (changed)
                onKioskChanged?.Invoke(this, Get(kioskUid));
        }

        public void SetMaxCountPerTransaction(string kioskUid, string sku, int limit) {
            bool changed = false;
            kioskUid = kioskUid.ToUpper();
            KioskEntity kiosk = _kioskRepository.Get(x => x.Uid == kioskUid).First();
            KioskProductLinkViewEntity link = kiosk.Links.FirstOrDefault(x => x.Sku == sku);

            if (link != null && link.MaxCountPerTransaction != limit) {
                _kioskProductLinkViewRepository.SetMaxCountPerTransaction(limit, link.Kiosk.Id, link.Sku);
                changed = true;
            }

            if (changed)
                onKioskChanged?.Invoke(this, Get(kioskUid));
        }

        public void SetMedia(string kioskUid, IEnumerable<KioskMediaLink> links) {
            KioskEntity kiosk = _kioskRepository.Get(x => x.Uid == kioskUid).First();
            Dictionary<string, IEnumerable<KioskMediaLink>> media = _getMedia(new string[] { kioskUid });

            bool changed = false;

            if (media.ContainsKey(kioskUid)) {
                IEnumerable<KioskMediaLink> existed = media[kioskUid];

                IEnumerable<KioskMediaLink> toDelete = existed.Where(x => !links.Any(l => l.Media.Hash == x.Media.Hash));
                if (toDelete.Any()) {
                    changed = true;
                    foreach (var link in toDelete)
                        _kioskRepository.DeleteMedia(kiosk.Id, link.Media.Hash);
                }

                IEnumerable<KioskMediaLink> toUpdate = existed.Where(x => links.Any(l => l.Media.Hash == x.Media.Hash && (x.Active != l.Active || x.Start != l.Start)));
                if (toUpdate.Any()) {
                    changed = true;
                    foreach (var link in toUpdate)
                        _kioskRepository.UpdateMedia(kiosk.Id, links.First(x => x.Media.Hash == link.Media.Hash));
                }

                IEnumerable<KioskMediaLink> toAdd = links.Where(x => !existed.Any(l => l.Media.Hash == x.Media.Hash));
                if (toAdd.Any()) {
                    changed = true;
                    foreach (var link in toAdd)
                        _kioskRepository.AddMedia(kiosk.Id, link);
                }
            }
            else {
                // Add media if there're none
                foreach (var link in links)
                    _kioskRepository.AddMedia(kiosk.Id, link);

                changed = true;
            }
            
            if (changed)
                onKioskChanged?.Invoke(this, Get(kioskUid));
        }

        public IEnumerable<Kiosk> GetKiosksWithSku(string sku)
            => Get(x => _kioskProductLinkViewRepository.Get(x => x.Sku == sku).Select(x => x.Kiosk.Uid).ToList().Distinct().Contains(x.UID));

        public IEnumerable<string> Extract(string kioskUid, IEnumerable<TransactionProductLink> cart) {
            PlanogramViewEntity? planogram = _planogramViewRepository.Get(x => x.KioskUid == kioskUid).FirstOrDefault();
            if (planogram == null)
                return new List<string>();

            PoG poG = PoG.Read(planogram.Planogram);
            List<string> result = new List<string>(); // list of addresses to dispense

            foreach (var item in cart) {
                for (int i = 0; i < item.Count; i++) {
                    List<PoGRoute>? routes = poG[item.Product.Sku].Routes?.OrderByDescending(x => x.Quantity).ToList();
                    if (routes != null && routes.Any()) {
                        PoGRoute route = routes.FirstOrDefault();
                        result.Add(route.Address);
                        route.Quantity--;
                    }
                }
            }

            PlanogramEntity? planogramEntity = _planogramRepository.Get(x=>x.KioskId == planogram.KioskId).FirstOrDefault();
            planogramEntity.Planogram = poG.ToString(false);
            _planogramRepository.Put(planogramEntity);

            return result;
        }

        private readonly KioskRepository _kioskRepository;
        private readonly KioskSettingsRepository _kioskSettingsRepository;
        private readonly KioskProductLinkViewRepository _kioskProductLinkViewRepository;
        private readonly PlanogramRepository _planogramRepository;
        private readonly PlanogramViewRepository _planogramViewRepository;
        private readonly Func<IEnumerable<string>, IEnumerable<Product>> _getProducts;
        private readonly Func<IEnumerable<string>, Dictionary<string, IEnumerable<KioskMediaLink>>> _getMedia;
    }
}