namespace Assimalign.Extensions.DependencyInjection.Internal;

internal enum CallSiteKind
{
    Factory,
    Constructor,
    Constant,
    Enumerable,
    ServiceProvider,
    Scope,
    Transient,
    Singleton
}
