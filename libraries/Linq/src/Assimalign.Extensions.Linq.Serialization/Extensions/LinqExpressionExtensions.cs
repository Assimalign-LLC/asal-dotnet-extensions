using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Assimalign.Extensions.Linq.Serialization.Nodes;
using Assimalign.Extensions.Linq.Serialization.Factories;


namespace Assimalign.Extensions.Linq.Serialization;

/// <summary>
/// Expression extension methods.
/// </summary>
public static class LinqExpressionExtensions
{
    /// <summary>
    /// Converts an expression to an expression node.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="factorySettings">The factory settings to use.</param>
    /// <returns></returns>
    //public static ExpressionNode ToExpressionNode(this Expression expression, LinqExpressionNodeFactorySettings factorySettings = null)
    //{
    //    var converter = new ExpressionConverter();
    //    return converter.Convert(expression, factorySettings);
    //}

    

    

    

    /// <summary>
    /// Gets the default factory.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <param name="factorySettings">The factory settings to use.</param>
    /// <returns></returns>
    internal static ILinqExpressionNodeFactory GetDefaultFactory(this Expression expression, LinqExpressionNodeFactorySettings factorySettings)
    {
        if (expression is LambdaExpression lambda)
            return new LinqExpressionNodeFactoryDefault(lambda.Parameters.Select(p => p.Type), factorySettings);
        return new LinqExpressionNodeFactory(factorySettings);
    }

    /// <summary>
    /// Gets the link nodes of an expression tree.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns></returns>
    internal static IEnumerable<Expression> GetLinkNodes(this Expression expression)
    {
        switch (expression)
        {
            case LambdaExpression lambdaExpression:
            {
                yield return lambdaExpression.Body;
                foreach (var parameter in lambdaExpression.Parameters)
                    yield return parameter;
                break;
            }
            case BinaryExpression binaryExpression:
                yield return binaryExpression.Left;
                yield return binaryExpression.Right;
                break;
            case ConditionalExpression conditionalExpression:
                yield return conditionalExpression.IfTrue;
                yield return conditionalExpression.IfFalse;
                yield return conditionalExpression.Test;
                break;
            case InvocationExpression invocationExpression:
            {
                yield return invocationExpression.Expression;
                foreach (var argument in invocationExpression.Arguments)
                    yield return argument;
                break;
            }
            case ListInitExpression listInitExpression:
                yield return listInitExpression.NewExpression;
                break;
            case MemberExpression memberExpression:
                yield return memberExpression.Expression;
                break;
            case MemberInitExpression memberInitExpression:
                yield return memberInitExpression.NewExpression;
                break;
            case MethodCallExpression methodCallExpression:
            {
                foreach (var argument in methodCallExpression.Arguments)
                    yield return argument;
                if (methodCallExpression.Object != null)
                    yield return methodCallExpression.Object;
                break;
            }
            case NewArrayExpression newArrayExpression:
            {
                foreach (var item in newArrayExpression.Expressions)
                    yield return item;
                break;
            }
            case NewExpression newExpression:
            {
                foreach (var item in newExpression.Arguments)
                    yield return item;
                break;
            }
            case TypeBinaryExpression typeBinaryExpression:
                yield return typeBinaryExpression.Expression;
                break;
            case UnaryExpression unaryExpression:
                yield return unaryExpression.Operand;
                break;
        }
    }

    /// <summary>
    /// Gets the nodes of an expression tree.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns></returns>
    internal static IEnumerable<Expression> GetNodes(this Expression expression)
    {
        foreach (var node in expression.GetLinkNodes())
        {
            foreach (var subNode in node.GetNodes())
                yield return subNode;
        }
        yield return expression;
    }

    /// <summary>
    /// Gets the nodes of an expression tree of given expression type.
    /// </summary>
    /// <typeparam name="TExpression">The type of the expression.</typeparam>
    /// <param name="expression">The expression.</param>
    /// <returns></returns>
    internal static IEnumerable<TExpression> GetNodes<TExpression>(this Expression expression) where TExpression : Expression
    {
        return expression.GetNodes().OfType<TExpression>();
    }
}
