


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class SelfCircularDependency
    {
        public SelfCircularDependency(SelfCircularDependency self)
        {

        }
    }
}
