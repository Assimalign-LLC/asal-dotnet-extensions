using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.ServiceLoopkup
{
    using Assimalign.Extensions.DependencyInjection.ServiceLoopkup.Kind;

    internal abstract class CallSiteVisitor<TArgument, TResult>
    {
        private readonly StackGuard stackGuard;

        protected CallSiteVisitor()
        {
            stackGuard = new StackGuard();
        }

        protected virtual TResult VisitCallSite(CallSiteService callSite, TArgument argument)
        {
            if (!stackGuard.TryEnterOnCurrentStack())
            {
                return stackGuard.RunOnEmptyStack((c, a) => VisitCallSite(c, a), callSite, argument);
            }

            switch (callSite.Cache.Location)
            {
                case CallSiteResultCacheLocation.Root:
                    return VisitRootCache(callSite, argument);
                case CallSiteResultCacheLocation.Scope:
                    return VisitScopeCache(callSite, argument);
                case CallSiteResultCacheLocation.Dispose:
                    return VisitDisposeCache(callSite, argument);
                case CallSiteResultCacheLocation.None:
                    return VisitNoCache(callSite, argument);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual TResult VisitCallSiteMain(CallSiteService callSite, TArgument argument)
        {
            switch (callSite.Kind)
            {
                case CallSiteKind.Factory:
                    return VisitFactory((FactoryCallSite)callSite, argument);
                case CallSiteKind.IEnumerable:
                    return VisitIEnumerable((EnumerableCallSite)callSite, argument);
                case CallSiteKind.Constructor:
                    return VisitConstructor((ConstructorCallSite)callSite, argument);
                case CallSiteKind.Constant:
                    return VisitConstant((ConstantCallSite)callSite, argument);
                case CallSiteKind.ServiceProvider:
                    return VisitServiceProvider((ServiceProviderCallSite)callSite, argument);
                default:
                    throw new NotSupportedException(SR.Format(SR.CallSiteTypeNotSupported, callSite.GetType()));
            }
        }

        protected virtual TResult VisitNoCache(CallSiteService callSite, TArgument argument)
        {
            return VisitCallSiteMain(callSite, argument);
        }

        protected virtual TResult VisitDisposeCache(CallSiteService callSite, TArgument argument)
        {
            return VisitCallSiteMain(callSite, argument);
        }

        protected virtual TResult VisitRootCache(CallSiteService callSite, TArgument argument)
        {
            return VisitCallSiteMain(callSite, argument);
        }

        protected virtual TResult VisitScopeCache(CallSiteService callSite, TArgument argument)
        {
            return VisitCallSiteMain(callSite, argument);
        }

        protected abstract TResult VisitConstructor(ConstructorCallSite constructorCallSite, TArgument argument);

        protected abstract TResult VisitConstant(ConstantCallSite constantCallSite, TArgument argument);

        protected abstract TResult VisitServiceProvider(ServiceProviderCallSite serviceProviderCallSite, TArgument argument);

        protected abstract TResult VisitIEnumerable(EnumerableCallSite enumerableCallSite, TArgument argument);

        protected abstract TResult VisitFactory(FactoryCallSite factoryCallSite, TArgument argument);
    }
}
