using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[CollectionDataContract(Name = "EIL")]
public class LinqExpressionElementInitNodeList : List<LinqExpressionElementInitNode>
{
    public LinqExpressionElementInitNodeList() { }

    public LinqExpressionElementInitNodeList(ILinqExpressionNodeFactory factory, IEnumerable<ElementInit> items)
    {
        if (factory == null)
            throw new ArgumentNullException("factory");
        if (items == null)
            throw new ArgumentNullException("items");
        AddRange(items.Select(item => new LinqExpressionElementInitNode(factory, item)));
    }

    internal IEnumerable<ElementInit> GetElementInits(ILinqExpressionContext context)
    {
        return this.Select(item => item.ToElementInit(context));
    }
}
