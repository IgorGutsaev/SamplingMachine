using Microsoft.EntityFrameworkCore;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Repositories
{
    public class KioskRepository : Repository<KioskEntity, int> {
        public KioskRepository(KioskDbContext context) : base(context) {
            _context = context;
        }

        public override IEnumerable<KioskEntity> Get(Func<KioskEntity, bool> predicate)
            => _context.Kiosks.Include(x => x.Settings).Include(x => x.Links).Where(predicate);

        public override void Put(IEnumerable<KioskEntity> entities)
            => throw new NotImplementedException();

        public void AddMedia(int kioskId, KioskMediaLink link) {
            link.Start = new DateTime(2000, 1, 1, link.Start.Hour, link.Start.Minute, link.Start.Second);
            lock (_context) {
                _context.Database.ExecuteSqlRaw($"INSERT INTO [KioskMediaLink] VALUES ({kioskId}, (SELECT TOP(1) Id FROM Media WHERE Hash LIKE '{link.Media.Hash}'), '{link.Start}', {Convert.ToInt32(link.Active)})");
            }
        }

        public void UpdateMedia(int kioskId, KioskMediaLink link) {
            link.Start = new DateTime(2000, 1, 1, link.Start.Hour, link.Start.Minute, link.Start.Second);
            lock (_context) {
                _context.Database.ExecuteSqlRaw($"UPDATE [KioskMediaLink] SET Start='{link.Start:yyyy-MM-dd HH:mm:ss.fff}', Active={Convert.ToInt32(link.Active)} WHERE KioskId={kioskId} AND MediaId=(SELECT TOP(1) Id FROM Media WHERE Hash LIKE '{link.Media.Hash}')");
            }
        }

        public void DeleteMedia(int kioskId, string hash) {
            lock (_context) {
                _context.Database.ExecuteSqlRaw($"DELETE FROM [KioskMediaLink] WHERE KioskId={kioskId} AND MediaId=(SELECT TOP(1) Id FROM Media WHERE Hash LIKE '{hash}')");
            }
        }

        private readonly KioskDbContext _context;
    }
}