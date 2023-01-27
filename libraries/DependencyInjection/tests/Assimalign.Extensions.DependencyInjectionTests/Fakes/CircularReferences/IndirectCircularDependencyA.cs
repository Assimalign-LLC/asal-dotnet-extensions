namespace Assimalign.Extensions.DependencyInjection.Tests.Fakes;

public class IndirectCircularDependencyA
{
    public IndirectCircularDependencyA(IndirectCircularDependencyB b)
    {

    }
}
