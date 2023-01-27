namespace Assimalign.Extensions.DependencyInjection.Tests.Fakes;

public class DirectCircularDependencyA
{
    public DirectCircularDependencyA(DirectCircularDependencyB b)
    {

    }
}
