using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class Transaction
    {
        [JsonPropertyName("phone")]
        public string PhoneNumber { get; set; }
        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }
        [JsonPropertyName("items")]
        public IEnumerable<TransactionProductLink> Items { get; set; }
    }
}