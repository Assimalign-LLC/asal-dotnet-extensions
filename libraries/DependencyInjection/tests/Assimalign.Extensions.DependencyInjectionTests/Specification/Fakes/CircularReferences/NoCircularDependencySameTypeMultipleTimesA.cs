


namespace Assimalign.Extensions.DependencyInjection.Tests.Fakes
{
    //    A
    //  / | \
    // B  C  C
    // |
    // C
    public class NoCircularDependencySameTypeMultipleTimesA
    {
        public NoCircularDependencySameTypeMultipleTimesA(
            NoCircularDependencySameTypeMultipleTimesB b,
            NoCircularDependencySameTypeMultipleTimesC c1,
            NoCircularDependencySameTypeMultipleTimesC c2)
        {

        }
    }
}
