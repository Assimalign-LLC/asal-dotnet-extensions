using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "M")]
public class LinqExpressionMemberNode : LinqExpressionNode<MemberExpression>
{
    public LinqExpressionMemberNode() { }

    public LinqExpressionMemberNode(ILinqExpressionNodeFactory factory, MemberExpression expression)
        : base(factory, expression) { }


    [DataMember(EmitDefaultValue = false, Name = "E")]
    public LinqExpressionNode Expression { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "M")]
    public LinqExpressionMemberInfoNode Member { get; set; }

    protected override void Initialize(MemberExpression expression)
    {
        Expression = Factory.Create(expression.Expression);
        Member = new LinqExpressionMemberInfoNode(Factory, expression.Member);
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        var member = Member.ToMemberInfo(context);
        return System.Linq.Expressions.Expression.MakeMemberAccess(Expression?.ToExpression(context), member);
    }
}
