using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;


[Serializable]
[DataContract(Name = "TB")]
public class LinqExpressionTypeBinaryNode : LinqExpressionNode<TypeBinaryExpression>
{
    public LinqExpressionTypeBinaryNode() { }

    public LinqExpressionTypeBinaryNode(ILinqExpressionNodeFactory factory, TypeBinaryExpression expression)
        : base(factory, expression) { }


    [DataMember(EmitDefaultValue = false, Name = "E")]
    public LinqExpressionNode Expression { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "O")]
    public TypeNode TypeOperand { get; set; }


    protected override void Initialize(TypeBinaryExpression expression)
    {
        Expression = Factory.Create(expression.Expression);
        TypeOperand = Factory.Create(expression.TypeOperand);
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        switch (NodeType)
        {
            case ExpressionType.TypeIs:
                return System.Linq.Expressions.Expression.TypeIs(Expression.ToExpression(context), TypeOperand.ToType(context));
            case ExpressionType.TypeEqual:
                return System.Linq.Expressions.Expression.TypeEqual(Expression.ToExpression(context), TypeOperand.ToType(context));
            default:
                throw new NotSupportedException("unrecognised TypeBinaryExpression.NodeType " + Enum.GetName(typeof(ExpressionType), NodeType));
        }
    }
}
