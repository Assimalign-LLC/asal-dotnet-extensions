using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[CollectionDataContract(Name = "MIL")]
public class LinqExpressionMemberInfoNodeList : List<LinqExpressionMemberInfoNode>
{
    public LinqExpressionMemberInfoNodeList() { }

    public LinqExpressionMemberInfoNodeList(ILinqExpressionNodeFactory factory, IEnumerable<MemberInfo> items = null)
    {
        if (factory == null)
            throw new ArgumentNullException("factory");
        if (items != null)
            AddRange(items.Select(m => new LinqExpressionMemberInfoNode(factory, m)));
    }

    public IEnumerable<MemberInfo> GetMembers(ILinqExpressionContext context)
    {
        return this.Select(m => m.ToMemberInfo(context));
    }
}
