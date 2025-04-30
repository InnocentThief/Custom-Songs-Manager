using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSM.Framework.Helper
{
    public class CaseInsensitiveJsonStringEnumConverter(Dictionary<Type, Dictionary<string, object>>? customMappings = null) : JsonConverterFactory
    {
        private readonly Dictionary<Type, Dictionary<string, object>> customMappings = customMappings ?? [];

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var enumType = typeToConvert;
            return (JsonConverter)Activator.CreateInstance(
                typeof(CaseInsensitiveEnumConverter<>).MakeGenericType(enumType),
                BindingFlags.Instance | BindingFlags.Public,
                null,
                [customMappings.TryGetValue(enumType, out Dictionary<string, object>? value) ? value : null],
                null)!;
        }

        private class CaseInsensitiveEnumConverter<T> : JsonConverter<T> where T : struct, Enum
        {
            private readonly Dictionary<string, T> _nameToValueMap;

            public CaseInsensitiveEnumConverter(Dictionary<string, object>? customMapping)
            {
                _nameToValueMap = typeof(T)
                    .GetFields()
                    .Where(f => f.IsStatic)
                    .ToDictionary(
                        f => f.Name,
                        f => (T)f.GetValue(null)!,
                        StringComparer.OrdinalIgnoreCase
                    );

                // Add custom mappings if provided
                if (customMapping != null)
                {
                    foreach (var mapping in customMapping)
                    {
                        _nameToValueMap[mapping.Key] = (T)mapping.Value;
                    }
                }
            }

            public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType != JsonTokenType.String)
                {
                    throw new JsonException();
                }

                var enumText = reader.GetString();

                if (enumText != null && _nameToValueMap.TryGetValue(enumText, out T value))
                {
                    return value;
                }

                throw new JsonException($"Unable to convert \"{enumText}\" to Enum \"{typeof(T)}\".");
            }

            public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }
    }
}

