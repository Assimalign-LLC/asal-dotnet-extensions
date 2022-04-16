using System;
using System.IO;
using System.Text.Json;

namespace Assimalign.Extensions.Validation.Configurable;

using Assimalign.Extensions.Validation;
using Assimalign.Extensions.Validation.Configurable.Serialization;

/// <summary>
/// A set of extensions for converting <see cref="IValidationConfigurableBuilder"/> into 
/// a <see cref="IValidator"/>.
/// </summary>
public static class ValidationConfigurableJsonExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">
    /// Represents the type in which the JSOM rules that are being 
    /// DeSerialized from <paramref name="json"/> will be applied to.
    /// </typeparam>
    /// <param name="builder"></param>
    /// <param name="json"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IValidationConfigurableBuilder AddJsonSource<T>(this IValidationConfigurableBuilder builder, string json, JsonSerializerOptions options = null)
        where T : class
    {
        return builder.Add(new ValidationConfigurableJsonSource<T>(() =>
        {
            options ??= GetDefaultJsonSerializationOptions();
            options.Converters.Add(new EnumConverter<OperatorType>());
            options.Converters.Add(new EnumConverter<ValidationConfigurableItemType>());
            options.Converters.Add(new EnumConverter<ValidationMode>());
            return JsonSerializer.Deserialize<ValidationConfigurableJsonProfile<T>>(json, options);
        }));
    }
  
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">
    /// Represents the type in which the JSOM rules that are being 
    /// DeSerialized from <paramref name="stream"/> will be applied to.
    /// </typeparam>
    /// <param name="builder"></param>
    /// <param name="stream"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IValidationConfigurableBuilder AddJsonSource<T>(this IValidationConfigurableBuilder builder, Stream stream, JsonSerializerOptions options = null)
        where T : class
    {
        return builder.Add(new ValidationConfigurableJsonSource<T>(() =>
        {
            options ??= GetDefaultJsonSerializationOptions();
            options.Converters.Add(new EnumConverter<OperatorType>());
            options.Converters.Add(new EnumConverter<ValidationConfigurableItemType>());
            options.Converters.Add(new EnumConverter<ValidationMode>());
            return JsonSerializer.DeserializeAsync<ValidationConfigurableJsonProfile<T>>(stream, options)
                .GetAwaiter()
                .GetResult();
        }));
    }

    private static JsonSerializerOptions GetDefaultJsonSerializationOptions()
    {
        return new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            ReadCommentHandling = JsonCommentHandling.Skip
        };
    }
}