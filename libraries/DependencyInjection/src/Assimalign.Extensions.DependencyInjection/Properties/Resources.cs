using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.DependencyInjection.Properties
{
    internal partial class Resources
    {


        internal static string GetConstantCantBeConvertedToServiceType(Type defaultType, Type serviceType)
        {
            return Format(ConstantCantBeConvertedToServiceType, defaultType, serviceType);
        }
        internal static string GetImplementationTypeCantBeConvertedToServiceType(Type implementationType, Type serviceType)
        {
            return Format(ImplementationTypeCantBeConvertedToServiceType, implementationType, serviceType);
        }
        internal static string GetCircularDependencyExceptionMessage(string typeName)
        {
            return string.Empty;
        }
    }
}
