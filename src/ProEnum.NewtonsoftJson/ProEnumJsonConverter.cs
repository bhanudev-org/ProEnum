using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ProEnum.NewtonsoftJson
{
    public class ProEnumJsonConverter : JsonConverter<ProEnum>
    {
        public override void WriteJson(JsonWriter writer, ProEnum value, JsonSerializer serializer) => writer.WriteValue(value.Name);

        public override ProEnum ReadJson(JsonReader reader, Type objectType, ProEnum existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            (reader.TokenType switch
            {
                JsonToken.Integer => GetEnumerationFromJson(reader.Value.ToString(), objectType),
                JsonToken.String => GetEnumerationFromJson(reader.Value.ToString(), objectType),
                JsonToken.Null => null!,
                _ => throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing an ProEnum")
            })!;

        private static ProEnum GetEnumerationFromJson(string nameOrValue, Type objectType)
        {
            try
            {
                object result = default!;
                var methodInfo = typeof(ProEnum).GetMethod(nameof(ProEnum.TryGetFromValueOrName), BindingFlags.Static | BindingFlags.Public);

                if(methodInfo == null) throw new JsonSerializationException("Serialization is not supported");

                var genericMethod = methodInfo.MakeGenericMethod(objectType);

                var arguments = new[] {nameOrValue, result};

                genericMethod.Invoke(null, arguments);
                return (arguments[1] as ProEnum)!;
            }
            catch(Exception ex)
            {
                throw new JsonSerializationException($"Error converting value '{nameOrValue}' to a ProEnum.", ex);
            }
        }
    }
}