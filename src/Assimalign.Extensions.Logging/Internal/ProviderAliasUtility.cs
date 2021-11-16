using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Assimalign.Extensions.Logging
{
    public class ProviderAliasUtility
    {
        private const string AliasAttibuteTypeFullName = "Assimalign.Extensions.Logging.ProviderAliasAttribute";

        public static string GetAlias(Type providerType)
        {
            IList<CustomAttributeData> attributes = CustomAttributeData.GetCustomAttributes(providerType);

            for (int i = 0; i < attributes.Count; i++)
            {
                CustomAttributeData attributeData = attributes[i];
                if (attributeData.AttributeType.FullName == AliasAttibuteTypeFullName &&
                    attributeData.ConstructorArguments.Count > 0)
                {
                    CustomAttributeTypedArgument arg = attributeData.ConstructorArguments[0];

                    System.Diagnostics.Debug.Assert(arg.ArgumentType == typeof(string));

                    return arg.Value?.ToString();
                }
            }

            return null;
        }
    }
}
