


namespace Assimalign.Extensions.DependencyInjection.Tests.Fakes
{
    public class SelfCircularDependency
    {
        public SelfCircularDependency(SelfCircularDependency self)
        {

        }
    }
}
