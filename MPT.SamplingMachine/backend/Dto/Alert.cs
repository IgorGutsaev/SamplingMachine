using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class Alert {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("kiosk")]
        public string Kiosk { get; set; }
    }
}
