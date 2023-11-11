using MPT.Vending.Domains.SharedContext;

namespace MPT.Vending.Domains.Advertisement.Infrastructure.Entities
{
    public class KioskMediaLinkViewEntity : Entity<int>
    {
        public string KioskUid { get; set; }
        public int MediaId { get; set; }
        public AdMediaEntity Media { get; set; }
        public DateTime Start { get; set; }
        public bool Active { get; set; }
    }
}
