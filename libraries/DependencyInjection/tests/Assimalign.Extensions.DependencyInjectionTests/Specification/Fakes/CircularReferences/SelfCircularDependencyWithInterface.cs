


namespace Assimalign.Extensions.DependencyInjection.Tests.Fakes
{
    public class SelfCircularDependencyWithInterface : ISelfCircularDependencyWithInterface
    {
        public SelfCircularDependencyWithInterface(ISelfCircularDependencyWithInterface self)
        {

        }
    }
}
