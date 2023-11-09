using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Products.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Products.Infrastructure.Repositories
{
    public class KioskProductLinkRepository : Repository<KioskProductLinkEntity, int>
    {
        public KioskProductLinkRepository(CatalogDbContext context) : base (context) {
            _context = context;
        }

        public override IEnumerable<KioskProductLinkEntity> Get(Func<KioskProductLinkEntity, bool> predicate)
            => _context.KioskProductLinks.Where(predicate);

        public override void Put(IEnumerable<KioskProductLinkEntity> entities) {
            throw new NotImplementedException();
        }

        public IEnumerable<KioskProductLinkEntity> Get(string kioskUid, string sku)
            => _context.KioskProductLinks.FromSql($"SELECT * FROM KioskProductLink WHERE KioskId = (SELECT Id FROM Kiosk WHERE Uid LIKE {kioskUid}) AND ProductId = (SELECT Id FROM Product WHERE Sku LIKE {sku})").ToList();

        public void Link(string kioskUid, string sku)
            => _context.Database.ExecuteSqlRaw("EXEC dbo.LinkProductToKiosk @p0, @p1", kioskUid, sku);

        public void Unlink(string kioskUid, string sku)
            => _context.Database.ExecuteSqlRaw("EXEC dbo.UnlinkProductToKiosk @p0, @p1", kioskUid, sku);

        public void ToggleLink(string kioskUid, string sku, bool disabled)
            => _context.Database.ExecuteSqlRaw("EXEC dbo.ToggleProductLink @p0, @p1, @p2", kioskUid, sku, disabled);

        private readonly CatalogDbContext _context;
    }
}