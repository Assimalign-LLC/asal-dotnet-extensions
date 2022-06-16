using System;
using System.IO;

namespace Assimalign.Extensions.Text.Json.Schema;

public static class SchemaWriter
{
    public static void WriteSchema(Utf8JsonWriter writer, JsonSchema schema, Formatting formatting = Formatting.Indented)
    {
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Formatting = formatting
        };

        var serializer = JsonSerializer.Create(settings);
        serializer.ContractResolver = new JsonSchemaContractResolver();
        serializer.Converters.Add(
            new StringEnumConverter
            {
                CamelCaseText = true
            });

        serializer.Serialize(writer, schema);
    }

    public static void WriteSchema(
        TextWriter writer,
        JsonSchema schema,
        Formatting formatting = Formatting.Indented)
    {
        var stringWriter = new StringWriter();
        var jsonWriter = new JsonTextWriter(stringWriter);

        WriteSchema(jsonWriter, schema, formatting);

        // Change "$$ref" to "$ref" before we ask write it to the output.
        string output = RefProperty.ConvertToOutput(stringWriter.ToString());
        writer.Write(output);
    }
}
