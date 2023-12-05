using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class SignInRequest
    {
        [Required(AllowEmptyStrings = false)]
        [Email]
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false)]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
