using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Pro.Enum.NewtonsoftJson
{
    public class EnumerationJsonConverter : JsonConverter<Enumeration>
    {
        public override void WriteJson(JsonWriter writer, Enumeration value, JsonSerializer serializer) => writer.WriteValue(value.Name);

        public override Enumeration ReadJson(JsonReader reader, Type objectType, Enumeration existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var readerValue = reader.Value?.ToString() ?? string.Empty;

            if(string.IsNullOrWhiteSpace(readerValue))
            {
                throw new JsonSerializationException($"Unexpected nameOrValue {reader.Value} when parsing an Enumeration");
            }

            return (reader.TokenType switch
            {
                JsonToken.Integer => GetEnumerationFromJson(readerValue, objectType),
                JsonToken.String => GetEnumerationFromJson(readerValue, objectType),
                JsonToken.Null => null!,
                _ => throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing an Enumeration")
            })!;
        }

        private static Enumeration GetEnumerationFromJson(string nameOrValue, Type objectType)
        {
            try
            {
                object result = default!;
                var methodInfo = typeof(Enumeration).GetMethod(nameof(Enumeration.TryGetFromValueOrName), BindingFlags.Static | BindingFlags.Public);

                if(methodInfo == null) throw new JsonSerializationException("Serialization is not supported");

                var genericMethod = methodInfo.MakeGenericMethod(objectType);

                var arguments = new[] { nameOrValue, result };

                genericMethod.Invoke(null, arguments);
                return (arguments[1] as Enumeration)!;
            }
            catch(Exception ex)
            {
                throw new JsonSerializationException($"Error converting value '{nameOrValue}' to a Enumeration.", ex);
            }
        }
    }
}