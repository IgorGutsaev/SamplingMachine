using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class SessionProductLink
    {
        [JsonPropertyName("product")]
        public Product Product { get; set; }
        [JsonPropertyName("count")]
        public int Count { get; set; }
        [JsonPropertyName("unitCredit")]
        public int UnitCredit { get; set; }
    }
}