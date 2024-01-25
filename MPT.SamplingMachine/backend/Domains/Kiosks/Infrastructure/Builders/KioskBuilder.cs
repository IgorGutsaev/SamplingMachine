using Filuet.Hardware.Dispensers.Abstractions.Models;
using Filuet.Infrastructure.Abstractions.Enums;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;
using System.Text.Json;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Builders
{
    public class KioskBuilder
    {
        private Kiosk kiosk = new Kiosk();

        public KioskBuilder WithData(KioskEntity entity, IEnumerable<Product>? products = null) {
            kiosk.UID = entity.Uid;
            kiosk.IsOn = entity.IsOn;

            foreach (var s in entity.Settings) {
                if (s.Identifier == "Credit")
                    kiosk.Credit = Convert.ToInt32(s.Value);
                else if (s.Identifier == "IdleTimeout")
                    kiosk.IdleTimeout = TimeSpan.Parse(s.Value);
                else if (s.Identifier == "Languages")
                    kiosk.Languages = JsonSerializer.Deserialize<IEnumerable<Language>>(s.Value);
            }

            kiosk.ProductLinks = entity.Links.Select(x => new KioskProductLink {
                Product = products?.FirstOrDefault(p => p.Sku == x.Sku) ?? new Product { Sku = x.Sku },
                Credit = x.Credit,
                MaxQtyPerTransaction = x.MaxCountPerTransaction,
                Disabled = x.Disabled
            });
            return this;
        }

        public KioskBuilder WithMedia(IEnumerable<KioskMediaLink>? links) {
            List<KioskMediaLink> media = new List<KioskMediaLink>();

            if (links != null) {
                foreach (var l in links)
                    media.Add(l);
            }

            kiosk.Media = media;

            return this;
        }

        public KioskBuilder WithPlanogram(PoG? planogram) {
            List<KioskProductLink> result = new List<KioskProductLink>();

            foreach (var p in kiosk.ProductLinks)
                result.Add(new KioskProductLink { Product = p.Product,
                    Credit = p.Credit,
                    Disabled = p.Disabled,
                    MaxQtyPerTransaction = p.MaxQtyPerTransaction,
                    RemainingQuantity = planogram.Products.FirstOrDefault(x => x.ProductUid == p.Product.Sku)?.Quantity ?? 0,
                    MaxQuantity = planogram.Products.FirstOrDefault(x => x.ProductUid == p.Product.Sku)?.MaxQuantity ?? 0
                });

            kiosk.ProductLinks = result;

            return this;
        }

        public Kiosk Build() => kiosk;
    }
}
