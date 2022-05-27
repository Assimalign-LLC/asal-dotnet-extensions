using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "MA")]   
public class LinqExpressionMemberAssignmentNode : LinqExpressionMemberBindingNode
{
    public LinqExpressionMemberAssignmentNode() { }

    public LinqExpressionMemberAssignmentNode(ILinqExpressionNodeFactory factory, MemberAssignment memberAssignment)
        : base(factory, memberAssignment.BindingType, memberAssignment.Member)
    {
        Expression = Factory.Create(memberAssignment.Expression);
    }


    [DataMember(EmitDefaultValue = false, Name = "E")]
    public LinqExpressionNode Expression { get; set; }

    internal override MemberBinding ToMemberBinding(ILinqExpressionContext context)
    {
        return System.Linq.Expressions.Expression.Bind(Member.ToMemberInfo(context), Expression.ToExpression(context));
    }
}
