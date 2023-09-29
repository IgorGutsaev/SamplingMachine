using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class Session
    {
        [JsonPropertyName("phone")]
        public string PhoneNumber { get; set; }
        [JsonPropertyName("date")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTimeOffset? Date { get; set; }
        [JsonPropertyName("items")]
        public IEnumerable<SessionProductLink> Items { get; set; }
    }
}