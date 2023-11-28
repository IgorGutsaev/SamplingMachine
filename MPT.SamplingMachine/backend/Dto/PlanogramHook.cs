using Filuet.Hardware.Dispensers.Abstractions.Models;
using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class PlanogramHook
    {
        [JsonPropertyName("kiosk")]
        public string KioskUid { get; set; }
        [JsonPropertyName("planogram")]
        public PoG Planogram { get; set; }
    }
}