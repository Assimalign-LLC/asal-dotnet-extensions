using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "MMB")]
public class LinqExpressionMemberMemberBindingNode : LinqExpressionMemberBindingNode
{
    public LinqExpressionMemberMemberBindingNode() { }

    public LinqExpressionMemberMemberBindingNode(ILinqExpressionNodeFactory factory, MemberMemberBinding memberMemberBinding)
        : base(factory, memberMemberBinding.BindingType, memberMemberBinding.Member)
    {
        Bindings = new LinqExpressionMemberBindingNodeList(factory, memberMemberBinding.Bindings);
    }


    [DataMember(EmitDefaultValue = false, Name = "B")]
    public LinqExpressionMemberBindingNodeList Bindings { get; set; }

    internal override MemberBinding ToMemberBinding(ILinqExpressionContext context)
    {
        return Expression.MemberBind(Member.ToMemberInfo(context), Bindings.GetMemberBindings(context));
    }
}
