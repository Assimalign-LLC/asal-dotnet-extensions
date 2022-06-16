namespace Assimalign.Extensions.Text.Json.Schema;

public static class JsonValueKindExtensions
{
    public static JsonSchemaType ToSchemaType(this JsonValueKind jsonValueKind)
    {
        switch (jsonValueKind)
        {
            case JsonValueKind.Array:
                return JsonSchemaType.Array;

            case JsonValueKind.True:
                return JsonSchemaType.Boolean;

            case JsonValueKind.False:
                return JsonSchemaType.Boolean;

            case JsonValueKind.Number:
                return JsonSchemaType.Number;

            case JsonValueKind.Null:
                return JsonSchemaType.Null;

            case JsonValueKind.Object:
                return JsonSchemaType.Object;

            case JsonValueKind.String:
                return JsonSchemaType.String;

            default:
                return JsonSchemaType.None;                 
        }
    }
}
