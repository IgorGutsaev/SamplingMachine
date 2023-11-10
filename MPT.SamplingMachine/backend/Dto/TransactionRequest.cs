using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class TransactionRequest
    {
        [JsonPropertyName("from")]
        public DateTime From { get; set; }
        [JsonPropertyName("to")]
        public DateTime To { get; set; }
        [JsonPropertyName("number")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull)]
        public string? MobileNumber { get; set; }
    }
}