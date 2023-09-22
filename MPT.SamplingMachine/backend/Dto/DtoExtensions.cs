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

            return target;
        }
    }
}
