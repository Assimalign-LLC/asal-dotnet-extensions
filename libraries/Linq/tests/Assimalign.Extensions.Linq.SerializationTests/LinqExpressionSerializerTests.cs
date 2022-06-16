using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Assimalign.Extensions.LinqTests;

using Assimalign.Extensions.Linq.Serialization;
using Assimalign.Extensions.Linq.Serialization.Factories;
using Assimalign.Extensions.LinqTests.Internal;

public class LinqExpressionSerializerTests
{
    public TestContext TestContext { get; set; }

    [Fact]
    public void CanSerializeTextTest()
    {
        foreach (var textSerializer in CreateTextSerializers())
        {
            var serializer = new ExpressionSerializer(textSerializer);
            Assert.True(serializer.CanSerializeText, "'{0}' was expected to serialize text.", textSerializer.GetType());
        }

        foreach (var binSerializer in CreateBinarySerializers())
        {
            var serializer = new ExpressionSerializer(binSerializer);
            Assert.False(serializer.CanSerializeText, "'{0}' was not expected to serialize text.", serializer.GetType());
        }
    }

    [Fact]
    public void CanSerializeBinaryTest()
    {
        foreach (var textSerializer in CreateTextSerializers())
        {
            var serializer = new ExpressionSerializer(textSerializer);
            Assert.False(serializer.CanSerializeBinary, "'{0}' was not expected to serialize binary.", textSerializer.GetType());
        }

        foreach (var binSerializer in CreateBinarySerializers())
        {
            var serializer = new ExpressionSerializer(binSerializer);
            Assert.True(serializer.CanSerializeBinary, "'{0}' was expected to serialize binary.", serializer.GetType());
        }
    }

    [Fact]
    public void SerializeDeserializeTextTest()
    {
        foreach (var textSerializer in CreateTextSerializers())
        {
            var serializer = new ExpressionSerializer(textSerializer);
            foreach (var expected in SerializerTestData.TestExpressions)
            {
                var text = serializer.SerializeText(expected);

                TestContext.WriteLine("{0} serializes to text with length {1}: {2}", expected, text.Length, text);

                var actual = serializer.DeserializeText(text);

                if (expected == null)
                {
                    Assert.IsNull(actual, "Input expression was null, but output is {0} for '{1}'", actual, textSerializer.GetType());
                    continue;
                }
                Assert.NotNull(actual, "Input expression was {0}, but output is null for '{1}'", expected, textSerializer.GetType());
                ExpressionAssert.AreEqual(expected, actual);
            }
        }
    }

    [Fact]
    public void SerializeDeserializeBinaryTest()
    {
        foreach (var binSerializer in CreateBinarySerializers())
        {
            var serializer = new ExpressionSerializer(binSerializer);
            foreach (var expected in SerializerTestData.TestExpressions)
            {
                var bytes = serializer.SerializeBinary(expected);

                TestContext.WriteLine("{0} serializes to bytes with length {1}", expected, bytes.Length);

                var actual = serializer.DeserializeBinary(bytes);

                if (expected == null)
                {
                    Assert.Null(actual, "Input expression was null, but output is {0} for '{1}'", actual, binSerializer.GetType());
                    continue;
                }
                Assert.NotNull(actual, "Input expression was {0}, but output is null for '{1}'", expected, binSerializer.GetType());
                ExpressionAssert.AreEqual(expected, actual);
            }
        }
    }

    [Fact]
    public void SerializeDeserializeBinaryComplexExpressionWithCompileTest()
    {
        foreach (var binSerializer in CreateBinarySerializers())
        {
            var serializer = new ExpressionSerializer(binSerializer);

            var expected = (Expression<Func<Bar, bool>>)(p => p.LastName == "Miller" && p.FirstName.StartsWith("M"));
            expected.Compile();

            var bytes = serializer.SerializeBinary(expected);
            TestContext.WriteLine("{0} serializes to bytes with length {1}", expected, bytes.Length);

            var actual = (Expression<Func<Bar, bool>>)serializer.DeserializeBinary(bytes);
            Assert.IsNotNull(actual, "Input expression was {0}, but output is null for '{1}'", expected, binSerializer.GetType());
            ExpressionAssert.AreEqual(expected, actual);

            actual.Compile();
        }
    }

    [Fact]
    public void NullableDecimalTest()
    {
        foreach (var textSerializer in CreateTextSerializers())
        {
            var serializer = new ExpressionSerializer(textSerializer);
            var expected = Expression.Constant(0m, typeof(Decimal?));

            var text = serializer.SerializeText(expected);

            TestContext.WriteLine("{0} serializes to text with length {1}: {2}", expected, text.Length, text);

            var actual = serializer.DeserializeText(text);
            Assert.IsNotNull(actual, "Input expression was {0}, but output is null for '{1}'", expected, textSerializer.GetType());
            ExpressionAssert.AreEqual(expected, actual);
        }
    }

    [Fact]
    public void SerializeNewObjWithoutParameters()
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());

        Expression<Func<List<int>, List<int>>> exp = l => new List<int>();

        var result = serializer.SerializeText(exp);
        Assert.IsNotNull(result);
    }

    [Fact]
    public void SerializeFuncExpressionsWithoutParameters()
    {
        var serializer = new ExpressionSerializer(new JsonSerializer());

        Expression<Func<bool>> exp = () => false;

        var result = serializer.SerializeText(exp);
        Assert.IsNotNull(result);
    }

    [Fact]
    public void SerializeDeserializeGuidValueAsJson()
    {
        SerializeDeserializeExpressionAsText(CreateGuidExpression(), new JsonSerializer());
    }

    [Fact]
    public void SerializeDeserializeGuidValueAsXml()
    {
        SerializeDeserializeExpressionAsText(CreateGuidExpression(), new XmlSerializer());
    }

    [Fact]
    public void SerializeDeserializeGuidValueAsBinary()
    {
        SerializeDeserializeExpressionAsBinary(CreateGuidExpression(), new BinarySerializer());
    }

    [Fact]
    public void ExpressionWithConstantDateTimeAsJson()
    {
        SerializeDeserializeExpressionAsText(CreateConstantDateTimeExpression(), new JsonSerializer());
    }

    [Fact]
    public void ExpressionWithConstantDateTimeAsXml()
    {
        SerializeDeserializeExpressionAsText(CreateConstantDateTimeExpression(), new XmlSerializer());
    }

    [Fact]
    public void ExpressionWithConstantDateTimeOffsetAsBinary()
    {
        SerializeDeserializeExpressionAsBinary(CreateConstantDateTimeOffsetExpression(), new BinarySerializer());
    }

    [Fact]
    public void ExpressionWithConstantDateTimeOffsetAsJson()
    {
        SerializeDeserializeExpressionAsText(CreateConstantDateTimeOffsetExpression(), new JsonSerializer());
    }

    [Fact]
    public void ExpressionWithConstantDateTimeOffsetAsXml()
    {
        SerializeDeserializeExpressionAsText(CreateConstantDateTimeOffsetExpression(), new XmlSerializer());
    }

    [Fact]
    public void ExpressionWithConstantDateTimeAsBinary()
    {
        SerializeDeserializeExpressionAsBinary(CreateConstantDateTimeExpression(), new BinarySerializer());
    }

    [Fact]
    public void ExpressionWithConstantTypeAsJson()
    {
        SerializeDeserializeExpressionAsText(CreateConstantTypeExpression(), new JsonSerializer());
    }

    [Fact]
    public void ExpressionWithConstantTypeAsXml()
    {
        SerializeDeserializeExpressionAsText(CreateConstantTypeExpression(), new XmlSerializer());
    }

    [Fact]
    public void ExpressionWithConstantTypeAsBinary()
    {
        SerializeDeserializeExpressionAsBinary(CreateConstantTypeExpression(), new BinarySerializer());
    }

    private static ConstantExpression CreateConstantDateTimeExpression()
    {
        return Expression.Constant(DateTime.Today);
    }

    private static ConstantExpression CreateConstantDateTimeOffsetExpression()
    {
        return Expression.Constant(DateTimeOffset.Now);
    }

    private static Expression<Func<Guid>> CreateGuidExpression()
    {
        var guidValue = Guid.NewGuid();
        return () => guidValue;
    }

    private static ConstantExpression CreateConstantTypeExpression()
    {
        return Expression.Constant(typeof(string));
    }

    private static Expression SerializeDeserializeExpressionAsText(Expression expression, ISerializer serializer)
    {
        var expressionSerializer = new ExpressionSerializer(serializer);
        var serialized = expressionSerializer.SerializeText(expression);

        return expressionSerializer.DeserializeText(serialized);
    }

    private static Expression SerializeDeserializeExpressionAsBinary(Expression expression, ISerializer serializer)
    {
        var expressionSerializer = new ExpressionSerializer(serializer);
        var serialized = expressionSerializer.SerializeBinary(expression);

        return expressionSerializer.DeserializeBinary(serialized);
    }

    private static IEnumerable<ITextSerializer> CreateTextSerializers()
    {
        return new ITextSerializer[] { new JsonSerializer(), new XmlSerializer() };
    }

    private static IEnumerable<IBinarySerializer> CreateBinarySerializers()
    {
#if WINDOWS_UWP 
        return new IBinarySerializer[] { new BinarySerializer() };
#else
        return new IBinarySerializer[] { new BinarySerializer(), new BinarayFormatterSerializer() };
#endif
    }
}
