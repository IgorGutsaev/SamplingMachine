using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class ProductStock
    {
        [JsonPropertyName("product")]
        public string ProductUid { get; set; }

        [JsonPropertyName("qty")]
        public int Quantuty { get; set; }

        [JsonPropertyName("max")]
        public int MaxQuantuty { get; set; }
    }

    public class KioskStock
    {
        [JsonPropertyName("kiosk")]
        public string KioskUid { get; set; }

        [JsonPropertyName("stock")]
        public IEnumerable<ProductStock> Stock { get; set; }
    }
}
