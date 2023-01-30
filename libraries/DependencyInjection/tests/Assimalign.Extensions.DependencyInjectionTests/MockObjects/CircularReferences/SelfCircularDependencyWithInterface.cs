


namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class SelfCircularDependencyWithInterface : ISelfCircularDependencyWithInterface
    {
        public SelfCircularDependencyWithInterface(ISelfCircularDependencyWithInterface self)
        {

        }
    }
}
