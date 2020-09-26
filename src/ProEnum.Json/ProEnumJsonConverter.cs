using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProEnum.Json
{
    public class ProEnumJsonConverter : JsonConverter<ProEnum>
    {
        private const string _nameProperty = "Name";

        public override bool CanConvert(Type objectType) => objectType.IsSubclassOf(typeof(ProEnum));

        public override ProEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            reader.TokenType switch
            {
                JsonTokenType.Number => GetEnumerationFromJson(reader.GetString() ?? string.Empty, typeToConvert),
                JsonTokenType.String => GetEnumerationFromJson(reader.GetString() ?? string.Empty, typeToConvert),
                JsonTokenType.Null => null!,
                _ => throw new JsonException($"Unexpected token {reader.TokenType} when parsing the ProEnum.")
            };

        public override void Write(Utf8JsonWriter writer, ProEnum value, JsonSerializerOptions options)
        {
            var name = value.GetType().GetProperty(_nameProperty, BindingFlags.Public | BindingFlags.Instance);
            if(name == null) throw new JsonException($"Error while writing JSON for {value}");

            writer.WriteStringValue(name.GetValue(value).ToString());
        }

        private static ProEnum GetEnumerationFromJson(string nameOrValue, Type objectType)
        {
            try
            {
                object result = default!;
                var methodInfo = typeof(ProEnum).GetMethod(nameof(ProEnum.TryGetFromValueOrName), BindingFlags.Static | BindingFlags.Public);

                if(methodInfo == null) throw new JsonException("Serialization is not supported");

                var genericMethod = methodInfo.MakeGenericMethod(objectType);
                var arguments = new[] {nameOrValue, result};

                genericMethod.Invoke(null, arguments);
                return (arguments[1] as ProEnum)!;
            }
            catch(Exception ex)
            {
                throw new JsonException($"Error converting value '{nameOrValue}' to a ProEnum.", ex);
            }
        }
    }
}