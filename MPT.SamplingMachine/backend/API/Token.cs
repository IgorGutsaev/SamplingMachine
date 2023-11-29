using System.Text.Json.Serialization;

namespace API
{
    public class TokenGenerationRequest
    {
        [JsonPropertyName("userid")]
        public string UserId { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("customClaims")]
        public Dictionary<string, object> CustomClaims { get; set; }
    }
}
