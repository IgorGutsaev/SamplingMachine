using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Abstractions;

namespace MPT.Vending.Domains.Kiosks.Services
{
    public class KioskService : IKioskService
    {
        public KioskDto Get(string uid)
        {
            if (DemoData._kiosks.Any(x => x.UID == uid))
                return DemoData._kiosks.First(x => x.UID == uid);

            return null;
        }

        public IEnumerable<KioskDto> GetAll()
            => DemoData._kiosks;

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

        public void DeleteProductLink(string kioskUid, string sku)
        {
            if (DemoData._kiosks.Any(x => x.UID == kioskUid) && DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks.Any(x => x.Product.Sku == sku))
                DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks =
                    DemoData._kiosks.FirstOrDefault(x => x.UID == kioskUid).ProductLinks.Where(x => x.Product.Sku != sku);
        }
    }
}