// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.IO;
using Newtonsoft.Json;

namespace Assimalign.Extensions.Text.Json.Schema;

public static class JsonSchemaReader
{
    public static JsonSchema ReadSchema(TextReader reader, string filePath)
    {
        return ReadSchema(reader.ReadToEnd(), filePath);
    }

    public static JsonSchema ReadSchema(string jsonText, string filePath)
    {
        SchemaValidationErrorAccumulator.Instance.Clear();

        // Change "$ref" to "$$ref" before we ask Json.NET to deserialize it,
        // because Json.NET treats "$ref" specially.
        jsonText = RefProperty.ConvertFromInput(jsonText);


        JsonSchema schema;


        using (var jsonReader = new JsonTextReader(new StringReader(jsonText)))
        {
            try
            {
                schema = JsonSerializer.Deserialize<JsonSchema>();
            }
            catch (JsonReaderException ex)
            {
                throw new JsonSyntaxException(filePath, ex);
            }
            finally
            {
                if (SchemaValidationErrorAccumulator.Instance.HasErrors)
                {
                    throw SchemaValidationErrorAccumulator.Instance.ToException();
                }
            }
        }

        return schema;
    }
}
