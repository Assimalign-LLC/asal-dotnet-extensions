using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Validation.Configurable;

using Assimalign.Extensions.Validation.Configurable.Internal.Exceptions;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class JsonConfigValidationProfile<T> : IValidationProfile
    where T : class
{
    private bool isConfigured;

    /// <summary>
    /// The default constructor for Validation Configurable JSON.
    /// </summary>
    [JsonConstructor]
    public JsonConfigValidationProfile()
    {
        this.ValidationItems ??= new List<JsonConfigValidationItem<T>>();
        this.ValidationConditions ??= new List<JsonConfigValidationCondition<T>>();
    }

    /// <summary>
    /// A user friendly description for documentation purposes.
    /// </summary>
    [JsonPropertyName("$description")]
    public string Description { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// This property is ignored on deserialization.
    /// </remarks>
    [JsonIgnore]
    public Type ValidationType => typeof(T);

    /// <summary>
    /// A collection of items to 
    /// </summary>
    [JsonPropertyName("$validationItems")]
    public IEnumerable<JsonConfigValidationItem<T>> ValidationItems { get; set; }
    IValidationItemStack IValidationProfile.ValidationItems { get; } = new ValidationItemStack();

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("$validationConditions")]
    public IEnumerable<JsonConfigValidationCondition<T>> ValidationConditions { get; set; }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="descriptor"></param>
    public void Configure(IValidationRuleDescriptor descriptor)
    {
        if (isConfigured)
        {
            return;
        }
        try
        {
            foreach (var validationCondition in this.ValidationConditions)
            {
                var condition = validationCondition.GetCondition();

                foreach (var validationItem in validationCondition.ValidationItems)
                {
                    validationItem.Configure(condition);
                    descriptor.RuleFor(validationItem);
                }
            }
            foreach (var validationItem in this.ValidationItems)
            {
                validationItem.Configure();
                descriptor.RuleFor(validationItem);
            }

            isConfigured = true; // Let's set this so someone doesn't try to call this more than once
        }
        catch (Exception exception) when (exception is not ValidationConfigurableException)
        {
            throw ValidationConfigurableJsonInternalException.FromException(
                message: $"An unhandled exception was thrown while configuring {nameof(JsonConfigValidationProfile<T>)}.", 
                exception: exception);
        }        
    }
}