using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "FI")]
public class LinqExpressionFieldInfoNode : LinqExpressionMemberInfoNode<FieldInfo>
{
    public LinqExpressionFieldInfoNode() { }

    public LinqExpressionFieldInfoNode(ILinqExpressionNodeFactory factory, FieldInfo memberInfo)
        : base(factory, memberInfo) { }

    protected override IEnumerable<FieldInfo> GetMemberInfosForType(ILinqExpressionContext context, Type type)
    {
        return type.GetFields();
    }
}
