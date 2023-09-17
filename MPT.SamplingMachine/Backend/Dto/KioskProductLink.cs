using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class KioskProductLink
    {
        [JsonPropertyName("maxCountPerSession")]
        public int MaxCountPerSession { get; set; }
        [JsonPropertyName("remainingQuantity")]
        public int RemainingQuantity { get; set; }
        [JsonPropertyName("credit")]
        public int Credit { get; set; }
        [JsonPropertyName("product")]
        public ProductDto Product { get; set; }
    }
}
