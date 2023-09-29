using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class LoginRequest
    {
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("pin")]
        public string Pin { get; set; }
    }
}