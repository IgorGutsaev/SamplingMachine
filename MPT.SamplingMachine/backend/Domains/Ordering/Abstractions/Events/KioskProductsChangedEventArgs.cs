using MPT.Vending.API.Dto;

namespace MPT.Vending.Domains.Ordering.Abstractions.Events
{
    public class KioskProductsChangedEventArgs : EventArgs
    {
        public string KioskUid { get; set; }
        public IEnumerable<KioskProductLink> Links { get; set; }
    }
}