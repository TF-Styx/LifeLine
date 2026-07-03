using Shared.Serialization.JsonConverters;
using System.Text.Json;

namespace Shared.Serialization.Extensions
{
    public static class JsonOptionsExtensions
    {
        public static JsonSerializerOptions AddTerminexDefault(this JsonSerializerOptions options)
        {
            options.PropertyNameCaseInsensitive = true;
            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.Converters.Add(new ErrorCodeJsonConverter());
            options.Converters.Add(new ErrorJsonConverter());

            return options;
        }
    }
}
