using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "I")]
public class LinqExpressionInvocationNode : LinqExpressionNode<InvocationExpression>
{
    public LinqExpressionInvocationNode() { }

    public LinqExpressionInvocationNode(ILinqExpressionNodeFactory factory, InvocationExpression expression)
        : base(factory, expression) { }


    [DataMember(EmitDefaultValue = false, Name = "A")]
    public LinqExpressionNodeList Arguments { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "E")]
    public LinqExpressionNode Expression { get; set; }

    protected override void Initialize(InvocationExpression expression)
    {
        Arguments = new LinqExpressionNodeList(Factory, expression.Arguments);
        Expression = Factory.Create(expression.Expression);
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        return System.Linq.Expressions.Expression.Invoke(Expression.ToExpression(context), Arguments.GetExpressions(context));
    }
}
