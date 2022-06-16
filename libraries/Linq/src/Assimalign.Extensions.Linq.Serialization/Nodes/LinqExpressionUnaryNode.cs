using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "U")]
public class LinqExpressionUnaryNode : LinqExpressionNode<UnaryExpression>
{
    public LinqExpressionUnaryNode() { }

    public LinqExpressionUnaryNode(ILinqExpressionNodeFactory factory, UnaryExpression expression)
        : base(factory, expression) { }


    [DataMember(EmitDefaultValue = false, Name = "O")]
    public LinqExpressionNode Operand { get; set; }

    protected override void Initialize(UnaryExpression expression)
    {
        Operand = Factory.Create(expression.Operand);
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        return NodeType == ExpressionType.UnaryPlus
            ? Expression.UnaryPlus(Operand.ToExpression(context))
            : Expression.MakeUnary(NodeType, Operand.ToExpression(context), Type.ToType(context));
    }
}
