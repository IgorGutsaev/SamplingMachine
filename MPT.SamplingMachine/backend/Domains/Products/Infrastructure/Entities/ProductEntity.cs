using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Products.Infrastructure.Entities
{
    public class ProductEntity : Entity<int>
    {
        public string Sku { get; set; }
        public int PictureId { get; set; }
        public PictureEntity Picture { get; set; }
    }
}