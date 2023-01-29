


namespace Assimalign.Extensions.DependencyInjection.Tests.Fakes
{
    public class SelfCircularDependencyGeneric<TDependency>
    {
        public SelfCircularDependencyGeneric(SelfCircularDependencyGeneric<string> dependency)
        {

        }

        public SelfCircularDependencyGeneric()
        {

        }
    }
}
