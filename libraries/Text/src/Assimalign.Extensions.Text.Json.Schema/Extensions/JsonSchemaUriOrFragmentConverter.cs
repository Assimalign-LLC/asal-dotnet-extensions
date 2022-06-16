using System;

namespace Assimalign.Extensions.Text.Json.Schema
{
    /// <summary>
    /// Converts a property of type <see cref="JsonSchemaUriOrFragment"/> to or from a string
    /// during serialization or deserialization.
    /// </summary>
    internal class JsonSchemaUriOrFragmentConverter : JsonConverter<JsonSchemaUriOrFragment>
    {
        public static readonly JsonSchemaUriOrFragmentConverter Instance = new JsonSchemaUriOrFragmentConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(JsonSchemaUriOrFragment);
        }

        public override JsonSchemaUriOrFragment? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new JsonSchemaUriOrFragment(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, JsonSchemaUriOrFragment value, JsonSerializerOptions options)
        {
            writer.WriteRawValue('"' + ((JsonSchemaUriOrFragment)value).ToString() + '"');
        }
    }
}