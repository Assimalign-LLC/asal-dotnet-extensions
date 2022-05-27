using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Linq.SerializationTests.Internal;

internal class Foo
{
    public string Name { get; set; }

    public bool IsFoo;

    public string GetName()
    {
        return this.Name;
    }
}