using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class ProductPictureUpdateRequest
    {
        [JsonPropertyName("sku")]
        public string Sku { get; set; }

        [JsonPropertyName("picture")]
        public string Picture { get; set; }
    }
}