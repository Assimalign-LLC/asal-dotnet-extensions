using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "NA")]
public class LinqExpressionNewArrayNode : LinqExpressionNode<NewArrayExpression>
{
    public LinqExpressionNewArrayNode() { }

    public LinqExpressionNewArrayNode(ILinqExpressionNodeFactory factory, NewArrayExpression expression)
        : base(factory, expression) { }

   
    [DataMember(EmitDefaultValue = false, Name = "E")]
    public LinqExpressionNodeList Expressions { get; set; }

    protected override void Initialize(NewArrayExpression expression)
    {
        Expressions = new LinqExpressionNodeList(Factory, expression.Expressions);
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        switch (NodeType)
        {
            case ExpressionType.NewArrayBounds:
                return Expression.NewArrayBounds(Type.ToType(context).GetElementType(), Expressions.GetExpressions(context));

            case ExpressionType.NewArrayInit:
                return Expression.NewArrayInit(Type.ToType(context).GetElementType(), Expressions.GetExpressions(context));

            default:
                throw new InvalidOperationException("Unhandeled nody type: " + NodeType);
        }
    }
}
