namespace MPT.Vending.API.Dto
{
    public static class DtoExtensions
    {
        public static KioskDto Merge(this KioskDto target, KioskDto source)
        {
            target.Credit = source.Credit;
            target.IdleTimeout = source.IdleTimeout;
            target.Languages = source.Languages;
            target.IsOn = source.IsOn;
            target.ProductLinks = source.ProductLinks;
            target.IsOn = source.IsOn;

            return target;
        }

        public static KioskDto OptimizeForCommunication(this KioskDto source)
            => new KioskDto {
                UID = source.UID,
                Credit = source.Credit,
                IdleTimeout = source.IdleTimeout,
                Languages = source.Languages,
                ProductLinks = source.ProductLinks.Select(x => new KioskProductLink
                {
                    Credit = x.Credit,
                    Disabled = x.Disabled,
                    MaxCountPerSession = x.MaxCountPerSession,
                    RemainingQuantity = x.RemainingQuantity,
                    Product = new ProductDto
                    {
                        Names = x.Product.Names,
                        Sku = x.Product.Sku
                    }
                }),
                IsOn = source.IsOn
            };
    }
}
