using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class User
    {
        [JsonPropertyName("uid")]
        /// <summary>
        /// Unique identifier
        /// </summary>
        public Guid UID { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonPropertyName("admin")]
        public bool Admin { get; set; }
    }
}