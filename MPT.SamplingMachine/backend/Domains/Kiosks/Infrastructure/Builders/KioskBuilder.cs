﻿using Filuet.Infrastructure.Abstractions.Enums;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;
using System.Text.Json;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Builders
{
    public class KioskBuilder
    {
        private Kiosk kiosk = new Kiosk();

        public KioskBuilder WithData(KioskEntity entity) {
            kiosk.UID = entity.Uid;
            kiosk.IsOn = entity.IsOn;
            return this;
        }

        public KioskBuilder WithSettings(IEnumerable<KioskSettingsEntity> settings) {
            foreach (var s in settings) {
                if (s.Identifier == "Credit")
                    kiosk.Credit = Convert.ToInt32(s.Value);
                else if (s.Identifier == "IdleTimeout")
                    kiosk.IdleTimeout = TimeSpan.Parse(s.Value);
                else if (s.Identifier == "Languages")
                    kiosk.Languages = JsonSerializer.Deserialize<IEnumerable<Language>>(s.Value);
            }
            return this;
        }

        public KioskBuilder WithLinks(IEnumerable<KioskProductLinkViewEntity> links, IEnumerable<Product>? products = null) {
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
