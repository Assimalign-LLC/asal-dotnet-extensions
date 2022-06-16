
using System;
using System.Linq.Expressions;
using System.Reflection;

using Assimalign.Extensions.Linq.Serialization.Nodes;

namespace Assimalign.Extensions.Linq.Serialization.Factories
{
    public class LinqExpressionNodeFactory : ILinqExpressionNodeFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinqExpressionNodeFactory"/> class.
        /// </summary>
        public LinqExpressionNodeFactory()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinqExpressionNodeFactory"/> class.
        /// </summary>
        /// <param name="factorySettings">The factory settings to use.</param>
        public LinqExpressionNodeFactory(LinqExpressionNodeFactorySettings factorySettings)
        {
            Settings = factorySettings ?? new LinqExpressionNodeFactorySettings();
        }

        public LinqExpressionNodeFactorySettings Settings { get; }

        /// <summary>
        /// Creates an expression node from an expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Unknown expression of type  + expression.GetType()</exception>
        public virtual LinqExpressionNode Create(Expression expression)
        {
            if (expression == null)
                return null;

            return expression switch
            {
                BinaryExpression binaryExpression => new LinqExpressionBinaryNode(this, binaryExpression),
                ConditionalExpression conditionalExpression => new LinqExpressionConditionalNode(this, conditionalExpression),
                ConstantExpression constantExpression => new LinqExpressionConstantNode(this, constantExpression),
                DefaultExpression defaultExpression => new LinqExpressionNodeDefault(this, defaultExpression),
                InvocationExpression invocationExpression => new LinqExpressionInvocationNode(this, invocationExpression),
                IndexExpression indexExpression => new LinqExpressionIndexNode(this, indexExpression),
                LambdaExpression lambdaExpression => new LinqExpressionLambdaNode(this, lambdaExpression),
                ListInitExpression listInitExpression => new LinqExpressionListInitNode(this, listInitExpression),
                MemberExpression memberExpression => new LinqExpressionMemberNode(this, memberExpression),
                MemberInitExpression memberInitExpression => new LinqExpressionMemberInitNode(this, memberInitExpression),
                MethodCallExpression methodCallExpression => new LinqExpressionMethodCallNode(this, methodCallExpression),
                NewArrayExpression newArrayExpression => new LinqExpressionNewArrayNode(this, newArrayExpression),
                NewExpression newExpression => new LinqExpressionNewNode(this, newExpression),
                ParameterExpression parameterExpression => new LinqExpressionParameterNode(this, parameterExpression),
                TypeBinaryExpression typeBinaryExpression => new LinqExpressionTypeBinaryNode(this, typeBinaryExpression),
                UnaryExpression unaryExpression => new LinqExpressionUnaryNode(this, unaryExpression),
                _ => throw new ArgumentException("Unknown expression of type " + expression.GetType())
            };
        }

        /// <summary>
        /// Creates an type node from a type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public TypeNode Create(Type type)
        {
            return new TypeNode(this, type);
        }

        /// <summary>
        /// Gets binding flags to be used when accessing type members.
        /// </summary>
        public BindingFlags? GetBindingFlags()
        {
            if (!Settings.AllowPrivateFieldAccess)
                return null;

            return BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        }
    }
}