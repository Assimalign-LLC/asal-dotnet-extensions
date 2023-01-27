namespace Assimalign.Extensions.LifeCycle;

/// <summary>
/// 
/// </summary>
public interface IStateStoreFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stateStoreType"></param>
    /// <returns></returns>
    IStateStore Create(StateStoreType stateStoreType);
}
