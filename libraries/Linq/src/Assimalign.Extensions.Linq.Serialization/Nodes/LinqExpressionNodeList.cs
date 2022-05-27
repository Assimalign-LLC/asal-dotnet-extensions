using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[CollectionDataContract(Name = "EL")]
public class LinqExpressionNodeList : List<LinqExpressionNode>
{
    public LinqExpressionNodeList() { }

    public LinqExpressionNodeList(ILinqExpressionNodeFactory factory, IEnumerable<Expression> items)
    {
        if (factory == null)
            throw new ArgumentNullException("factory");
        if (items == null)
            throw new ArgumentNullException("items");
        AddRange(items.Select(factory.Create));
    }

    internal IEnumerable<Expression> GetExpressions(ILinqExpressionContext context)
    {
        return this.Select(e => e.ToExpression(context));
    }

    internal IEnumerable<ParameterExpression> GetParameterExpressions(ILinqExpressionContext context)
    {
        return this.OfType<LinqExpressionParameterNode>().Select(e => (ParameterExpression)e.ToExpression(context));
    }
}
