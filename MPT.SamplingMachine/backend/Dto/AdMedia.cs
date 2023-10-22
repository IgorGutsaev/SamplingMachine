using MPT.Vending.API.Dto.converters;
using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class AdMedia
    {        
        /// <summary>
        /// Unique id
        /// </summary>
        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        [JsonConverter(typeof(AdMediaTypeJsonConverter))]
        public AdMediaType Type { get; set; }
    }
}