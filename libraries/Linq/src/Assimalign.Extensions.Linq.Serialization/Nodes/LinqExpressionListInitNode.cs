using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;


[Serializable]
[DataContract(Name = "LI")]
public class LinqExpressionListInitNode : LinqExpressionNode<ListInitExpression>
{
    public LinqExpressionListInitNode() { }

    public LinqExpressionListInitNode(ILinqExpressionNodeFactory factory, ListInitExpression expression)
        : base(factory, expression) { }


    [DataMember(EmitDefaultValue = false, Name = "I")]
    public LinqExpressionElementInitNodeList Initializers { get; set; }

   
    [DataMember(EmitDefaultValue = false, Name = "N")]
    public LinqExpressionNode NewExpression { get; set; }

    protected override void Initialize(ListInitExpression expression)
    {
        Initializers = new LinqExpressionElementInitNodeList(Factory, expression.Initializers);
        NewExpression = Factory.Create(expression.NewExpression);
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        return Expression.ListInit((NewExpression)NewExpression.ToExpression(context), Initializers.GetElementInits(context));
    }
}
