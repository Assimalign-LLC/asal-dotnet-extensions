using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Assimalign.Extensions.Linq.SerializationTests;

using Assimalign.Extensions.Linq.SerializationTests.Internal;
using Assimalign.Extensions.Linq.Serialization.Extensions;

public class LinqExpressionMemberExtensionsTests
{

    [Fact]
    public void GetReturnTypeOfPropertyTest()
    {
        
        var actual = typeof(Foo).GetProperty("Name").GetReturnType();
        Assert.Equal(typeof(string), actual);
    }

    [Fact]
    public void GetReturnTypeOfFieldTest()
    {
        var actual = typeof(Foo).GetField("IsFoo").GetReturnType();
        Assert.Equal(typeof(bool), actual);
    }

    [Fact]
    public void GetReturnTypeOfMethodTest()
    {
        var actual = typeof(Foo).GetMethod("GetName").GetReturnType();
        Assert.Equal(typeof(string), actual);
    }
}
