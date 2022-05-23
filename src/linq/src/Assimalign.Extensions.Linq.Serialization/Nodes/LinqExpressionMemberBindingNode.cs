using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "MB")]
public abstract class LinqExpressionMemberBindingNode : Node
{
    protected LinqExpressionMemberBindingNode() { }

    protected LinqExpressionMemberBindingNode(ILinqExpressionNodeFactory factory)
        : base(factory) { }

    protected LinqExpressionMemberBindingNode(ILinqExpressionNodeFactory factory, MemberBindingType bindingType, MemberInfo memberInfo)
        : base(factory)
    {
        BindingType = bindingType;
        Member = new LinqExpressionMemberInfoNode(Factory, memberInfo);
    }
    

    [DataMember(EmitDefaultValue = false, Name = "BT")]
    public MemberBindingType BindingType { get; set; }

  
    [DataMember(EmitDefaultValue = false, Name = "M")]
    public LinqExpressionMemberInfoNode Member { get; set; }

    internal abstract MemberBinding ToMemberBinding(ILinqExpressionContext context);

    internal static LinqExpressionMemberBindingNode Create(ILinqExpressionNodeFactory factory, MemberBinding memberBinding)
    {
        return memberBinding switch
        {
            MemberAssignment => new LinqExpressionMemberAssignmentNode(factory, (MemberAssignment)memberBinding),
            MemberListBinding => new LinqExpressionMemberListBindingNode(factory, (MemberListBinding)memberBinding),
            MemberMemberBinding => new LinqExpressionMemberMemberBindingNode(factory, (MemberMemberBinding)memberBinding),
            not null => throw new ArgumentException("Unknown member binding of type " + memberBinding.GetType(), "memberBinding"),
            _ => null
        };
    }
}
