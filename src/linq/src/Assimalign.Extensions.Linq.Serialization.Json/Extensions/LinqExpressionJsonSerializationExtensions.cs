using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Assimalign.Extensions.Linq.Serialization;

using Assimalign.Extensions.Linq.Serialization.Nodes;
using Assimalign.Extensions.Linq.Serialization.Factories;

public static class LinqExpressionJsonSerializationExtensions
{
    /// <summary>
    /// Converts an expression to an json encoded string.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="factorySettings">The factory settings to use.</param>
    /// <returns></returns>
    public static string ToJson(this Expression expression, LinqExpressionNodeFactorySettings factorySettings = null)
    {
        return expression.ToJson(expression.GetDefaultFactory(factorySettings));
    }

    /// <summary>
    /// Converts an expression to an json encoded string using the given factory.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="factory">The factory.</param>
    /// <returns></returns>
    public static string ToJson(this Expression expression, ILinqExpressionNodeFactory factory)
    {
        return expression.ToJson(factory, new LinqExpressionJsonSerializer());
    }

    /// <summary>
    /// Converts an expression to an json encoded string using the given factory and serializer.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="factory">The factory.</param>
    /// <param name="serializer">The serializer.</param>
    /// <returns></returns>
    public static string ToJson(this Expression expression, ILinqExpressionNodeFactory factory, ILinqExpressionJsonSerializer serializer)
    {
        return expression.ToText(factory, serializer);
    }
}
