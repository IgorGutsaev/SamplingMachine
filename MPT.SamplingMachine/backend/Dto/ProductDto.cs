using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class ProductDto
    {
        [JsonPropertyName("sku")]
        public string Sku { get; set; }
        [JsonPropertyName("names")]
        public IEnumerable<LocalizedValue> Names { get; set; }
        [JsonPropertyName("picture")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Picture { get; set; }
    }
}