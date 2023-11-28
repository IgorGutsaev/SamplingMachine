using Filuet.Hardware.Dispensers.Abstractions.Models;

namespace MPT.Vending.Domains.Kiosks.Abstractions.Events
{
    public class PlanogramChangeEventArgs : EventArgs
    {
        public string KioskUid { get; set; }
        public PoG Planogram{ get; set; }
    }
}