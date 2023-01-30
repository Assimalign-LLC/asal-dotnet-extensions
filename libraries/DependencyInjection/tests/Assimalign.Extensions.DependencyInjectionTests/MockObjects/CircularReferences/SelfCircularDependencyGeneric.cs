


namespace Assimalign.Extensions.DependencyInjection.MockObjects
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
