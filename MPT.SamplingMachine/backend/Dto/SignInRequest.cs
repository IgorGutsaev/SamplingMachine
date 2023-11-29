using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class SignInRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
