using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "MLB")]
public class LinqExpressionMemberListBindingNode : LinqExpressionMemberBindingNode
{
    public LinqExpressionMemberListBindingNode() { }

    public LinqExpressionMemberListBindingNode(ILinqExpressionNodeFactory factory, MemberListBinding memberListBinding)
        : base(factory, memberListBinding.BindingType, memberListBinding.Member)
    {
        Initializers = new LinqExpressionElementInitNodeList(Factory, memberListBinding.Initializers);
    }


    [DataMember(EmitDefaultValue = false, Name = "I")]
    public LinqExpressionElementInitNodeList Initializers { get; set; }

    internal override MemberBinding ToMemberBinding(ILinqExpressionContext context)
    {
        return Expression.ListBind(Member.ToMemberInfo(context), Initializers.GetElementInits(context));
    }
}
