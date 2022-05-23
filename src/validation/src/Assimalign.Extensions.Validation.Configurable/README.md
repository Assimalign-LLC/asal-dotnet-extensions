# Assimalign Extensions Validation configurable

This library offers an abstraction for implementing a custom configurable validator. With the available Assimalign Extensions library there is already a based implementation for implementing an XML and JSON configurable binary. However, there are some limitations to the available rules that can be implemented on a type.


# Getting Started
Below is a quick start guide for implementing a custom Validation Configuration Provider. The provider 

### 1. Build a Custom Validation Profile
First add a custom validation profile that can be configured when the IValidationConfigurableSource builds the provider.

```csharp 
public class ValidationConfigurableJsonProfile<T> : IValidationProfile
{
    public Type ValidationType { get; }

    public ValidationMode ValidationMode { get; }

    public IValidationItemStack ValidationItems { get; }

    public void Configure(IValidationRuleDescriptor descriptor) 
    {
        // Some logic for configuring the JSON Validation Profile
    }
}

```

### 2. Create a Configurable Validation Source for the custom Profile


```csharp 
public sealed class ValidationConfigurableJsonSource<T> : IValidationConfigurableSource
    where T : class
{
    private readonly Func<ValidationConfigurableJsonProfile<T>> configure;

    public ValidationConfigurableJsonSource(Func<ValidationConfigurableJsonProfile<T>> configure)
    {
        this.configure = configure;
    }

    public IValidationConfigurableProvider Build() =>
        new ValidationConfigurableJsonProvider<T>(configure.Invoke());
}

```

### 3. Create a Configurable Validation Provider to injest the source

Let's note that a IValidationConfigurableSource can have multiple providers. Depending on the scenerio there may be times when the source of the validation rules changes locations such as local development vs a API request to retrieve the validaiton rules. 

```csharp 
public sealed class ValidationConfigurableJsonProvider<T> : IValidationConfigurableProvider
    where T : class
{
    private readonly ValidationConfigurableJsonProfile<T> profile;

    private ValidationConfigurableJsonProvider() { }

    internal ValidationConfigurableJsonProvider(ValidationConfigurableJsonProfile<T> profile)
    {
        this.profile = profile;
    }


    public IValidationProfile GetProfile() => this.profile;


    public bool TryGetProfile(Type type, out IValidationProfile profile)
    {
        if (type == typeof(T))
        {
            profile = this.profile;
            return true;
        }
        else
        {
            profile = null;
            return false;
        }
    }
}

```


### 4. Use the Configurable Validation Source to inject the provider

Now we should be able to build a configurable validator with our custom provider.

```csharp
 var validator = ValidationConfigurableBuilder.Create()
        .Add(new ValidationConfigurableJsonSource<T>(() =>
        {
            return JsonSerializer.Deserialize<{Some Serialialzable Validation Profile}>(json);
        }))
        .ToValidator();
```
