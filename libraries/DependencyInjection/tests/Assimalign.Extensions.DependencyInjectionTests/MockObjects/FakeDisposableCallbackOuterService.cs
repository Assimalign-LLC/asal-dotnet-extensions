


using System.Linq;
using System.Collections.Generic;

namespace Assimalign.Extensions.DependencyInjection.MockObjects
{
    public class FakeDisposableCallbackOuterService : FakeDisposableCallbackService, IFakeOuterService
    {
        public FakeDisposableCallbackOuterService(
            IFakeService singleService,
            IEnumerable<IFakeMultipleService> multipleServices,
            FakeDisposeCallback callback) : base(callback)
        {
            SingleService = singleService;
            MultipleServices = multipleServices.ToArray();
        }

        public IFakeService SingleService { get; }
        public IEnumerable<IFakeMultipleService> MultipleServices { get; }
    }
}
