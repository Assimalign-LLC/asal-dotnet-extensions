using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Xml.Extensions
{
    using Assimalign.Extensions.Xml.Serialization;

    internal static class XmlSerializerExtensions
    {
        internal static void AddBuiltInConverters(this XmlSerializerOptions options)
        {
            options.AddConverters(new XmlConverter[]
            {

            });
        }
    }
}
