


using System.Collections.Generic;

namespace Assimalign.Extensions.DependencyInjection.Specification.Fakes
{
    public interface IFakeOuterService
    {
        IFakeService SingleService { get; }

        IEnumerable<IFakeMultipleService> MultipleServices { get; }
    }
}
