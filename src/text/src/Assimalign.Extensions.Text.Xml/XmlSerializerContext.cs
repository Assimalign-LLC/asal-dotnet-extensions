
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Xml
{
    using Assimalign.Extensions.Xml.Serialization;

    internal sealed class XmlSerializerContext
    {
        public XmlSerializerContext()
        {
            this.Converters = new Dictionary<Type, XmlConverter>();
        }

        public IDictionary<Type, XmlConverter> Converters { get; set; }
    }
}
