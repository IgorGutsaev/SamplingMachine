using System.Text.Json.Serialization;

namespace webapi
{
    public class Product
    {
        [JsonPropertyName("sku")]
        public string Sku { get; set; }
        [JsonPropertyName("names")]
        public Dictionary<string, string> Names { get; set; }
        [JsonPropertyName("maxCountPerSession")]
        public int MaxCountPerSession { get; set; }
        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }
        [JsonPropertyName("picture")]
        public string Picture { get; set; }
    }
}
