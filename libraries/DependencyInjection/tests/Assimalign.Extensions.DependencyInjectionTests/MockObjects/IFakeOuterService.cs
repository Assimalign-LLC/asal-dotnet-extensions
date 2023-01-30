


using System.Collections.Generic;

namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public interface IFakeOuterService
    {
        IFakeService SingleService { get; }

        IEnumerable<IFakeMultipleService> MultipleServices { get; }
    }
}
