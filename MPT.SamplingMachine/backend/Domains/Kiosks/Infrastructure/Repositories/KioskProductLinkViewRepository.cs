using Microsoft.EntityFrameworkCore;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Repositories
{
    public class KioskProductLinkViewRepository : Repository<KioskProductLinkViewEntity, int>
    {
        public KioskProductLinkViewRepository(KioskDbContext context) : base (context) {
            _context = context;
        }

        public override IEnumerable<KioskProductLinkViewEntity> Get(Func<KioskProductLinkViewEntity, bool> predicate)
            => _context.KioskProductLinksView.Where(predicate);

        public override void Put(IEnumerable<KioskProductLinkViewEntity> entities) {
            throw new NotImplementedException();
        }

        public void SetCredit(int credit, int kioskId, string sku) {
            _context.Database.ExecuteSqlRaw($"UPDATE KioskProductLink SET Credit={credit} WHERE KioskId={kioskId} AND ProductId=(SELECT TOP(1) Id FROM Product WHERE Sku LIKE '{sku}')");
        }

        public void SetMaxCountPerTransaction(int count, int kioskId, string sku) {
            _context.Database.ExecuteSqlRaw($"UPDATE KioskProductLink SET MaxCountPerTransaction={count} WHERE KioskId={kioskId} AND ProductId=(SELECT TOP(1) Id FROM Product WHERE Sku LIKE '{sku}')");
        }

        private readonly KioskDbContext _context;
    }
}