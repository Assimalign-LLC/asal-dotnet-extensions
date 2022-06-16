using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;


[Serializable]
[DataContract(Name = "CI")]
public class LinqExpressionConstructorInfoNode : LinqExpressionMemberInfoNode<ConstructorInfo>
{
    public LinqExpressionConstructorInfoNode() { }

    public LinqExpressionConstructorInfoNode(ILinqExpressionNodeFactory factory, ConstructorInfo memberInfo)
        : base(factory, memberInfo) { }

    /// <summary>
    /// Gets the member infos for the specified type.
    /// </summary>
    /// <param name="context">The expression context.</param>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    protected override IEnumerable<ConstructorInfo> GetMemberInfosForType(ILinqExpressionContext context, Type type)
    {
        return type.GetConstructors();
    }
}
