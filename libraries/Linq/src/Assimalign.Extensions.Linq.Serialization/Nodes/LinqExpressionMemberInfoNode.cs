using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

using Assimalign.Extensions.Linq.Serialization.Exceptions;

[Serializable]
[DataContract(Name = "MI")]
public class LinqExpressionMemberInfoNode : LinqExpressionMemberInfoNode<MemberInfo>
{
    public LinqExpressionMemberInfoNode() { }

    public LinqExpressionMemberInfoNode(ILinqExpressionNodeFactory factory, MemberInfo memberInfo)
        : base(factory, memberInfo) { }

    protected override IEnumerable<MemberInfo> GetMemberInfosForType(ILinqExpressionContext context, Type type)
    {
        BindingFlags? flags = null;
        if (context != null)
            flags = context.GetBindingFlags();
        else if (Factory != null)
            flags = Factory.GetBindingFlags();
        return flags == null ? type.GetMembers() : type.GetMembers(flags.Value);
    }
}

[Serializable]
[DataContract(Name = "MN")]
public abstract class LinqExpressionMemberInfoNode<TMemberInfo> : Node where TMemberInfo : MemberInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinqExpressionMemberInfoNode{TMemberInfo}"/> class.
    /// </summary>
    protected LinqExpressionMemberInfoNode() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinqExpressionMemberInfoNode{TMemberInfo}"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="memberInfo">The member info.</param>
    protected LinqExpressionMemberInfoNode(ILinqExpressionNodeFactory factory, TMemberInfo memberInfo)
        : base(factory)
    {
        if (memberInfo != null)
            Initialize(memberInfo);
    }


    /// <summary>
    /// Gets or sets the type of the declaring.
    /// </summary>
    /// <value>
    /// The type of the declaring.
    /// </value>
    [DataMember(EmitDefaultValue = false, Name = "D")]
    public TypeNode DeclaringType { get; set; }


    /// <summary>
    /// Gets or sets the signature.
    /// </summary>
    /// <value>
    /// The signature.
    /// </value>
    [DataMember(EmitDefaultValue = false, Name = "S")]
    public string Signature { get; set; }

    /// <summary>
    /// Initializes the instance using specified member info.
    /// </summary>
    /// <param name="memberInfo">The member info.</param>
    protected virtual void Initialize(TMemberInfo memberInfo)
    {
        DeclaringType = Factory.Create(memberInfo.DeclaringType);
        Signature = memberInfo.ToString();
    }

    /// <summary>
    /// Gets the the declaring type.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    /// <exception cref="System.InvalidOperationException">DeclaringType is not set.</exception>
    /// <exception cref="System.TypeLoadException">Failed to load DeclaringType:  + this.DeclaringType</exception>
    protected Type GetDeclaringType(ILinqExpressionContext context)
    {
        if (DeclaringType == null)
            throw new InvalidOperationException("DeclaringType is not set.");

        var declaringType = DeclaringType.ToType(context);
        if (declaringType == null)
            throw new TypeLoadException("Failed to load DeclaringType: " + DeclaringType);

        return declaringType;
    }

    /// <summary>
    /// Converts this instance to an expression.
    /// </summary>
    /// <param name="context">The expression context.</param>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    protected abstract IEnumerable<TMemberInfo> GetMemberInfosForType(ILinqExpressionContext context, Type type);

    /// <summary>
    /// Converts this instance to a member info object of type TMemberInfo.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public virtual TMemberInfo ToMemberInfo(ILinqExpressionContext context)
    {
        if (string.IsNullOrWhiteSpace(Signature))
            return null;

        var declaringType = GetDeclaringType(context);
        var members = GetMemberInfosForType(context, declaringType);

        var member = members.FirstOrDefault(m => m.ToString() == Signature);
        if (member == null)
            throw new MemberNotFoundException("MemberInfo not found. See DeclaringType and MemberSignature properties for more details.",
                declaringType, Signature);
        return member;
    }
}

