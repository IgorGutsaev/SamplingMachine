using Filuet.Infrastructure.Abstractions.Enums;
using System.Text.Json.Serialization;

namespace MPT.Vending.API.Dto
{
    public class LocalizedValue
    {
        [JsonPropertyName("lang")]
        public Language Language { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }

        public static LocalizedValue Bind(Language language, string value)
            => new LocalizedValue { Language = language, Value = value };
    }
}