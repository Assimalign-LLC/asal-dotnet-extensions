using System;
using System.Text.Json.Serialization;

namespace Assimalign.Extensions.Validation.Configurable;

/// <summary>
/// Represents a configurable JSON validation error.
/// </summary>
public sealed class JsonConfigValidationError : IValidationError
{
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("$code")]
    public string Code { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("$message")]
    public string Message { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("$source")]
    public string Source { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"Error {Code}: {Message} {Environment.NewLine} └─> Source: {Source}";
    }
}
