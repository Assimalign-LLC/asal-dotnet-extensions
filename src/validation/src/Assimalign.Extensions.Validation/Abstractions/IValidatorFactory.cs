namespace Assimalign.Extensions.Validation;

/// <summary>
/// 
/// </summary>
public interface IValidatorFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="validatorName"></param>
    /// <returns></returns>
    IValidator Create(string validatorName);
}

