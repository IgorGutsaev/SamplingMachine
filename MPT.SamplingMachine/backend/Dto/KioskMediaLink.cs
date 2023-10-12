using Filuet.Infrastructure.Abstractions.Converters;
using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class KioskMediaLink
    {
        [JsonPropertyName("media")]
        public AdMedia Media { get; set; }
        [JsonPropertyName("start")]
        [JsonConverter(typeof(TimeOfDayJsonConverter))]
        public DateTime Start { get; set; }
        [JsonPropertyName("active")]
        public bool Active { get; set; }
    }
}
