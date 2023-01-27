namespace Assimalign.Extensions.LifeCycle;

public enum StateStoreType
{
    Default = 0,
    /// <summary>
    /// State managed through the lifecycle of the application
    /// </summary>
    Global = 1,
    Scoped = 2
}
