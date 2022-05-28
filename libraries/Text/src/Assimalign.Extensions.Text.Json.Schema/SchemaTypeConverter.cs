// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Assimalign.Extensions.Text.Json.Schema
{
    internal class SchemaTypeConverter : JsonConverter
    {
        public static SchemaTypeConverter Instance = new SchemaTypeConverter();

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(JsonSchemaType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JToken jToken = JToken.Load(reader);
            var schemaTypes = new List<JsonSchemaType>();

            if (jToken.Type == JTokenType.String)
            {
                string typeString = jToken.ToObject<string>();
                JsonSchemaType schemaType = SchemaTypeFromString(typeString);
                if (schemaType != JsonSchemaType.None)
                {
                    schemaTypes.Add(schemaType);
                }
                else
                {
                    SchemaValidationErrorAccumulator.Instance.AddError(jToken, ErrorNumber.InvalidTypeString, typeString);
                    return null;
                }
            }
            else if (jToken.Type == JTokenType.Array)
            {
                bool allValid = true;
                foreach (JToken elementToken in jToken as JArray)
                {
                    if (elementToken.Type == JTokenType.String)
                    {
                        string typeString = elementToken.ToObject<string>();
                        JsonSchemaType schemaType = SchemaTypeFromString(typeString);
                        if (schemaType != JsonSchemaType.None)
                        {
                            schemaTypes.Add(schemaType);
                        }
                        else
                        {
                            allValid = false;
                            SchemaValidationErrorAccumulator.Instance.AddError(elementToken, ErrorNumber.InvalidTypeString, typeString);
                        }
                    }
                    else
                    {
                        allValid = false;
                        SchemaValidationErrorAccumulator.Instance.AddError(elementToken, ErrorNumber.InvalidTypeType, elementToken.Type);
                    }
                }

                if (!allValid)
                {
                    return null;
                }
            }
            else
            {
                SchemaValidationErrorAccumulator.Instance.AddError(jToken, ErrorNumber.InvalidTypeType, jToken.Type);
                return null;
            }

            return schemaTypes;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (!(value is IEnumerable<JsonSchemaType> schemas))
            {
                return;
            }

            string[] types = schemas.Select(st => st.ToString().ToLowerInvariant()).ToArray();

            if (types.Length == 1)
            {
                writer.WriteValue(types[0]);
            }
            else
            {
                writer.WriteStartArray();
                foreach (string type in types)
                {
                    writer.WriteValue(type);
                }
                writer.WriteEndArray();
            }
        }

        private static JsonSchemaType SchemaTypeFromString(string s)
        {
            s = s.Substring(0, 1).ToUpperInvariant() + s.Substring(1);

            // Returns SchemaType.None if unrecognized.
            JsonSchemaType schemaType;
            Enum.TryParse(s, out schemaType);

            return schemaType;
        }
    }
}
