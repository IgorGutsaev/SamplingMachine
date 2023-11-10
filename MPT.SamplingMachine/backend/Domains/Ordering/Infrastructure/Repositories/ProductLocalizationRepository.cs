using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Ordering.Infrastructure.Entities;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Repositories
{
    public class ProductLocalizationRepository : Repository<ProductLocalizationEntity, int>
    {
        public ProductLocalizationRepository(CatalogDbContext context) : base (context) {
            _context = context;
        }

        public override IEnumerable<ProductLocalizationEntity> Get(Func<ProductLocalizationEntity, bool> predicate)
            => _context.ProductLocalizations.Where(predicate).Where(x => !x.Deleted);

        public IEnumerable<ProductLocalizationEntity> GetWithDeleted(Func<ProductLocalizationEntity, bool> predicate)
            => _context.ProductLocalizations.Where(predicate);

        public override void Put(IEnumerable<ProductLocalizationEntity> entities) {
            throw new NotImplementedException();
        }

        public IEnumerable<int> FindProductsByFilter(string filter)
            => _context.ProductLocalizations.Where(x => x.Value.Contains(filter)).Select(x=>x.ProductId).Distinct().ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="actualNames"></param>
        /// <returns>Indicates true when there're changes in localized names</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool MergeNames(int productId, IEnumerable<LocalizedValue>? actualNames) {
            if (actualNames == null || actualNames.Count() == 0)
                throw new ArgumentNullException("Product must have at least one name");

            bool changed = false;

            IEnumerable<ProductLocalizationEntity> stored = GetWithDeleted(x => x.ProductId == productId && x.Attribute == "name");

            // entries to delete- find all languages that aren't in the new list (not in the actualNames)
            IEnumerable<ProductLocalizationEntity> toDelete = stored.Where(x => !x.Deleted && !actualNames.Select(v => v.Language).Contains(x.Language));
            
            if (toDelete.Any()) {
                changed = true;
                toDelete.ToList().ForEach(x => x.Deleted = true); // mark as deleted so they are not returned by get request
                foreach (var d in toDelete)
                    _context.Update(d);
            }

            // entries to add- find all languages that are in the new list (in the actualNames) and not in the database
            IEnumerable<LocalizedValue>? newLanguages = actualNames.Where(x => !stored.Select(v => v.Language).Contains(x.Language));
            if (newLanguages != null && newLanguages.Any()) {
                changed = true;
                foreach (var l in newLanguages)
                    _context.Add(new ProductLocalizationEntity { ProductId = productId, Language = l.Language, Attribute = "name", Value = l.Value });
            }

            // commit changes
            if (changed)
                _context.SaveChanges();

            return changed;
        }

        private readonly CatalogDbContext _context;
    }
}