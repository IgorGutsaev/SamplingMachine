using MPT.Vending.API.Dto;
using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Advertisement.Infrastructure.Entities
{
    public class AdMediaEntity : Entity<int>
    {
        public string Hash { get; set; }
        public string Name { get; set; }
        public AdMediaType Type { get; set; }
        public int Size { get; set; }
    }
}
