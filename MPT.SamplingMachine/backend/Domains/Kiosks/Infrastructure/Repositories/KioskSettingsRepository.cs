using MPT.Vending.Domains.Kiosks.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Kiosks.Infrastructure.Repositories
{
    public class KioskSettingsRepository : Repository<KioskSettingsEntity, int>
    {
        public KioskSettingsRepository(KioskDbContext context) : base (context) {
            _context = context;
        }

        public override IEnumerable<KioskSettingsEntity> Get(Func<KioskSettingsEntity, bool> predicate)
            => _context.KioskSettings.Where(predicate);

        public override KioskSettingsEntity Put(KioskSettingsEntity entity) {
            if (entity.KioskId == 0 || string.IsNullOrWhiteSpace(entity.Identifier))
                throw new ArgumentException("Some arguments are missing");

            return base.Put(entity);
        }

        public override void Put(IEnumerable<KioskSettingsEntity> entities) {
            foreach (var e in entities)
                if (e.KioskId == 0 || string.IsNullOrWhiteSpace(e.Identifier))
                    throw new ArgumentException("Some settings are invalid");

            foreach (var e in entities) {
                if (e.Id > 0)
                    _context.Update(e);
                else _context.Add(e);
            }

            _context.SaveChanges();
        }

        private readonly KioskDbContext _context;
    }
}