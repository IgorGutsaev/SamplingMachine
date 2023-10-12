using MPT.Vending.API.Dto.converters;
using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class AdMedia
    {
        [JsonPropertyName("uid")]
        public Guid Uid { get; set; }

        [JsonPropertyName("type")]
        [JsonConverter(typeof(AdMediaTypeJsonConverter))]
        public AdMediaType Type { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }
    }
}
