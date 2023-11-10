using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class TransactionHookRequest
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
