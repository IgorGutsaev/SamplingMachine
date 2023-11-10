using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class KioskProductLink
    {
        [JsonPropertyName("maxQty")]
        public int MaxCountPerTransaction { get; set; }
        [JsonPropertyName("remains")]
        public int RemainingQuantity { get; set; }
        [JsonPropertyName("credit")]
        public int Credit { get; set; }
        [JsonPropertyName("product")]
        public Product Product { get; set; }
        [JsonPropertyName("disabled")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public bool Disabled { get; set; }
    }
}
