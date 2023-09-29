using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class Customer
    {
        [JsonPropertyName("number")]
        public string MobileNumber { get; set; }
    }
}
