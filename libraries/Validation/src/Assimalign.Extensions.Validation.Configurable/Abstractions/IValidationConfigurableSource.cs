namespace Assimalign.Extensions.Validation.Configurable;

/// <summary>
/// 
/// </summary>
public interface IValidationConfigurableSource
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IValidationConfigurableProvider Build();
}