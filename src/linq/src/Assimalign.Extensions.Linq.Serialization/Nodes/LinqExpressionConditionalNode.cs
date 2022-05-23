using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "IF")]
public class LinqExpressionConditionalNode : LinqExpressionNode<ConditionalExpression>
{
    public LinqExpressionConditionalNode() { }

    public LinqExpressionConditionalNode(ILinqExpressionNodeFactory factory, ConditionalExpression expression)
        : base(factory, expression) { }

    [DataMember(EmitDefaultValue = false, Name = "IFF")]
    public LinqExpressionNode IfFalse { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "IFT")]
    public LinqExpressionNode IfTrue { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "C")]
    public LinqExpressionNode Test { get; set; }

    /// <summary>
    /// Initializes the specified expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    protected override void Initialize(ConditionalExpression expression)
    {
        Test = Factory.Create(expression.Test);
        IfTrue = Factory.Create(expression.IfTrue);
        IfFalse = Factory.Create(expression.IfFalse);
    }

    /// <summary>
    /// Converts this instance to an expression.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public override Expression ToExpression(ILinqExpressionContext context)
    {
        return Expression.Condition(Test.ToExpression(context), IfTrue.ToExpression(context), IfFalse.ToExpression(context));
    }
}
