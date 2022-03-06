using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Linq.Serialization;

public static class LinqExpressionTextSerializationExtensions
{
    /// <summary>
    /// Converts an expression to an encoded string using the given factory and serializer.
    /// The encoding is decided by the serializer.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="factory">The factory.</param>
    /// <param name="serializer">The serializer.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">
    /// factory
    /// or
    /// serializer
    /// </exception>
    public static string ToText(this Expression expression, ILinqExpressionNodeFactory factory, ILinqExpressionTextSerializer serializer)
    {
        if (factory == null)
            throw new ArgumentNullException(nameof(factory));
        if (serializer == null)
            throw new ArgumentNullException(nameof(serializer));

        return serializer.Serialize(factory.Create(expression));
    }
}
