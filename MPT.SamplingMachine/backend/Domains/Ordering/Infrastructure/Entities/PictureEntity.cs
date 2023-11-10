using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Ordering.Infrastructure.Entities
{
    public class PictureEntity : Entity<int>
    {
        public Guid Uid { get; set; }
    }
}
