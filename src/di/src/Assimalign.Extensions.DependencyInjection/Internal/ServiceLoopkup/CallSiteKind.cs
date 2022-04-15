

namespace Assimalign.Extensions.DependencyInjection.ServiceLoopkup
{
    internal enum CallSiteKind
    {
        Factory,

        Constructor,

        Constant,

        IEnumerable,

        ServiceProvider,

        Scope,

        Transient,

        Singleton
    }
}
