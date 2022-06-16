using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[CollectionDataContract(Name = "MBL")]    
public class LinqExpressionMemberBindingNodeList : List<LinqExpressionMemberBindingNode>
{
    public LinqExpressionMemberBindingNodeList() { }

    public LinqExpressionMemberBindingNodeList(ILinqExpressionNodeFactory factory, IEnumerable<MemberBinding> items)
    {
        if (factory == null)
            throw new ArgumentNullException("factory");
        if (items == null)
            throw new ArgumentNullException("items");
        AddRange(items.Select(m => LinqExpressionMemberBindingNode.Create(factory, m)));
    }

    internal IEnumerable<MemberBinding> GetMemberBindings(ILinqExpressionContext context)
    {
        return this.Select(memberBindingEntity => memberBindingEntity.ToMemberBinding(context));
    }
}
