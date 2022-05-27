using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[DataContract(Name = "MIE")]

[Serializable]

public class LinqExpressionMemberInitNode : LinqExpressionNode<MemberInitExpression>
{
    public LinqExpressionMemberInitNode() { }

    public LinqExpressionMemberInitNode(ILinqExpressionNodeFactory factory, MemberInitExpression expression)
        : base(factory, expression) { }


    [DataMember(EmitDefaultValue = false, Name = "B")]
    public LinqExpressionMemberBindingNodeList Bindings { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "N")]
    public LinqExpressionNewNode NewExpression { get; set; }

    protected override void Initialize(MemberInitExpression expression)
    {
        Bindings = new LinqExpressionMemberBindingNodeList(Factory, expression.Bindings);
        NewExpression = (LinqExpressionNewNode)Factory.Create(expression.NewExpression);
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        return Expression.MemberInit((NewExpression)NewExpression.ToExpression(context), Bindings.GetMemberBindings(context));
    }
}