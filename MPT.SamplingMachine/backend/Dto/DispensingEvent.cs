using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class DispensingEvent
    {
        [JsonPropertyName("address")]
        public string Address { get; set; }
        [JsonPropertyName("kioskUid")]
        public string KioskUid { get; set; }
    }
}