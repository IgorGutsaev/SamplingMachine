using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class ServiceLoginRequest
    {
        [JsonPropertyName("pin")]
        public string Pin { get; set; }
    }
}