using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class ProductStock
    {
        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("qty")]
        public int Quantity { get; set; }

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
