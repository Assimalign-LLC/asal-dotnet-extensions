using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;


[Serializable]
[DataContract(Name = "D")]
public class LinqExpressionNodeDefault : LinqExpressionNode<DefaultExpression>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinqExpressionNodeDefault"/> class.
    /// </summary>
    public LinqExpressionNodeDefault() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinqExpressionNodeDefault"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="expression">The expression.</param>
    public LinqExpressionNodeDefault(ILinqExpressionNodeFactory factory, DefaultExpression expression)
        : base(factory, expression) { }

    protected override void Initialize(DefaultExpression expression)
    {
       // nothing to do
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        return Expression.Default(Type.ToType(context));
    }
}
