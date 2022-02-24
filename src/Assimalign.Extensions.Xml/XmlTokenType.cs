using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Xml
{
    [Flags]
    public enum XmlTokenType
    {
        Element = 0,
        Attribute = 1,
        RootStart,
        RootEnd
        
    }
}
