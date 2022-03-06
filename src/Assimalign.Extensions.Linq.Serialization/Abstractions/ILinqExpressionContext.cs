using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Assimalign.Extensions.Linq.Serialization;

using Assimalign.Extensions.Linq.Serialization.Nodes;

public interface ILinqExpressionContext
{
    BindingFlags? GetBindingFlags();

    ParameterExpression GetParameterExpression(LinqExpressionParameterNode node);

    Type ResolveType(TypeNode node);
}
