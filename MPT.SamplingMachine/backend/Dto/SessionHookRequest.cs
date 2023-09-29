using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class SessionHookRequest
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
