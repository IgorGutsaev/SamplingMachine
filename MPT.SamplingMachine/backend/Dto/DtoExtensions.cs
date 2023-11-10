namespace MPT.Vending.API.Dto
{
    public static class DtoExtensions
    {
        public static Kiosk Merge(this Kiosk target, Kiosk source) {
            target.Credit = source.Credit;
            target.IdleTimeout = source.IdleTimeout;
            target.Languages = source.Languages;
            target.IsOn = source.IsOn;
            target.ProductLinks = source.ProductLinks;
            target.IsOn = source.IsOn;

            return target;
        }

        public static Kiosk OptimizeForCommunication(this Kiosk source)
            => new Kiosk {
                UID = source.UID,
                Credit = source.Credit,
                IdleTimeout = source.IdleTimeout,
                Languages = source.Languages,
                ProductLinks = source.ProductLinks.Select(x => new KioskProductLink {
                    Credit = x.Credit,
                    Disabled = x.Disabled,
                    MaxCountPerTransaction = x.MaxCountPerTransaction,
                    RemainingQuantity = x.RemainingQuantity,
                    Product = new Product {
                        Names = x.Product.Names,
                        Sku = x.Product.Sku
                    }
                }),
                IsOn = source.IsOn,
                Media = source.Media.Where(x => x.Active)
            };

        public static Kiosk PrepareForCommunication(this Kiosk source) {
            source.Media = source.Media.Where(x => x.Active);
            return source;
        }
    }
}