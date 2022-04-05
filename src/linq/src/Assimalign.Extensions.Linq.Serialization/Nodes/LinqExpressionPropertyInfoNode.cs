using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "PI")]
public class LinqExpressionPropertyInfoNode : LinqExpressionMemberInfoNode<PropertyInfo>
{
    public LinqExpressionPropertyInfoNode() { }

    public LinqExpressionPropertyInfoNode(ILinqExpressionNodeFactory factory, PropertyInfo memberInfo)
        : base(factory, memberInfo) { }

    protected override IEnumerable<PropertyInfo> GetMemberInfosForType(ILinqExpressionContext context, Type type)
    {
        return type.GetProperties();
    }
}
