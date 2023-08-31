using Filuet.Infrastructure.Abstractions.Business;
using Filuet.Infrastructure.Abstractions.Enums;
using System.Text.Json.Serialization;

namespace webapi
{
    public class Product
    {
        [JsonPropertyName("sku")]
        public string Sku { get; set; }
        [JsonPropertyName("names")]
        public Dictionary<string, string> Names { get; set; }
        [JsonPropertyName("price")]
        public Money Price { get; set; }
        [JsonPropertyName("picture")]
        public string Picture { get; set; }
    }
}
