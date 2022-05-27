
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Linq.Serialization;

using Assimalign.Extensions.Linq.Serialization.Factories;

public static class LinqExpressionXmlSerializationExtensions
{
    /// <summary>
    /// Converts an expression to an xml encoded string.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="factorySettings">The factory settings to use.</param>
    /// <returns></returns>
    public static string ToXml(this Expression expression, LinqExpressionNodeFactorySettings factorySettings = null)
    {
        return expression.ToXml(expression.GetDefaultFactory(factorySettings));
    }

    /// <summary>
    /// Converts an expression to an xml encoded string using the given factory.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="factory">The factory.</param>
    /// <returns></returns>
    public static string ToXml(this Expression expression, ILinqExpressionNodeFactory factory)
    {
        return expression.ToXml(factory, new LinqExpressionXmlSerializer());
    }

    /// <summary>
    /// Converts an expression to an xml encoded string using the given factory and serializer.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="factory">The factory.</param>
    /// <param name="serializer">The serializer.</param>
    /// <returns></returns>
    public static string ToXml(this Expression expression, ILinqExpressionNodeFactory factory, ILinqExpressionXmlSerializer serializer)
    {
        return expression.ToText(factory, serializer);
    }
}
