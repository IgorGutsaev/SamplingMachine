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
                MaxCountPerTransaction = x.MaxCountPerTransaction,
                Disabled = x.Disabled
            });
            return this;
        }

        public KioskBuilder WithStock(IEnumerable<KioskProductLinkViewEntity> links, IEnumerable<Product>? products = null) {
            kiosk.ProductLinks = links.Select(x => new KioskProductLink {
                Product = products?.FirstOrDefault(p => p.Sku == x.Sku) ?? new Product { Sku = x.Sku },
                Credit = x.Credit,
                MaxCountPerTransaction = x.MaxCountPerTransaction,
                //RemainingQuantity = , // to be done
                Disabled = x.Disabled
            });
            return this;
        }

        public Kiosk Build() => kiosk;
    }
}
