using System.Text.Json;
using System.Text.Json.Serialization;
using Terminex.Common.Results;

namespace Shared.Serialization.JsonConverters
{
    public class ErrorJsonConverter : JsonConverter<Error>
    {
        public override Error? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;

            var errorCode = JsonSerializer.Deserialize<ErrorCode>(root.GetProperty("errorCode").GetRawText(), options);
            var message = root.GetProperty("message").GetString();

            return new Error(errorCode, message!);
        }

        public override void Write(Utf8JsonWriter writer, Error value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("errorCode");
            JsonSerializer.Serialize(writer, value.ErrorCode, options);
            writer.WriteString("message", value.Message);
            writer.WriteEndObject();
        }
    }
}
