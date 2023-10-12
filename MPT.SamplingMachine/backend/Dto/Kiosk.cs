using Filuet.Infrastructure.Abstractions.Enums;
using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class Kiosk
    {
        [JsonPropertyName("uid")]
        /// <summary>
        /// Unique identifier
        /// </summary>
        public string UID { get; set; }
        [JsonPropertyName("languages")]
        public IEnumerable<Language> Languages { get; set; }
        [JsonPropertyName("credit")]
        public int Credit { get; set; }
        [JsonPropertyName("idleTimeout")]
        public TimeSpan IdleTimeout { get; set; }
        [JsonPropertyName("links")]
        public IEnumerable<KioskProductLink> ProductLinks { get; set; }
        [JsonPropertyName("media")]
        public IEnumerable<KioskMediaLink> Media { get; set; }
        [JsonPropertyName("isOn")]
        public bool IsOn { get; set; }
    }
}