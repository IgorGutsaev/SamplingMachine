using Filuet.Infrastructure.Abstractions.Helpers;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace MPT.Vending.API.Dto.converters
{
    public class AdMediaTypeJsonConverter : JsonConverter<AdMediaType>
    {
        public override AdMediaType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => EnumHelpers.GetValueFromCode<AdMediaType>(reader.GetString());

        public override void Write(Utf8JsonWriter writer, AdMediaType mtype, JsonSerializerOptions options)
            => writer.WriteStringValue(mtype.GetCode());
    }
}