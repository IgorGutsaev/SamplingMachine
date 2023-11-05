using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Products.Infrastructure.Entities
{
    public class PictureEntity : Entity<int>
    {
        public Guid Uid { get; set; }
    }
}
