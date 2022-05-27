
using System;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Collections.Concurrent;


namespace Assimalign.Extensions.Linq.Serialization;

using Assimalign.Extensions.Linq.Serialization.Nodes;

public abstract class LinqExpressionContextBase : ILinqExpressionContext
{
    private readonly ConcurrentDictionary<string, ParameterExpression> _parameterExpressions;
    private readonly ConcurrentDictionary<string, Type> _typeCache;

    protected LinqExpressionContextBase()
    {
        _parameterExpressions = new ConcurrentDictionary<string, ParameterExpression>();
        _typeCache = new ConcurrentDictionary<string, Type>();
    }

    public bool AllowPrivateFieldAccess { get; set; }

    public virtual BindingFlags? GetBindingFlags()
    {
        if (!AllowPrivateFieldAccess)
            return null;

        return BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
    }

    public virtual ParameterExpression GetParameterExpression(LinqExpressionParameterNode node)
    {
        if (node == null)
            throw new ArgumentNullException(nameof(node));
        var key = node.Type.Name + Environment.NewLine + node.Name;
        return _parameterExpressions.GetOrAdd(key, k => Expression.Parameter(node.Type.ToType(this), node.Name));
    }

    public virtual Type ResolveType(TypeNode node)
    {
        if (node == null)
            throw new ArgumentNullException(nameof(node));

        if (string.IsNullOrWhiteSpace(node.Name))
            return null;

        return _typeCache.GetOrAdd(node.Name, n =>
        {
            var type = Type.GetType(n);
            if (type == null)
            {
                foreach (var assembly in GetAssemblies())
                {
                    type = assembly.GetType(n);
                    if (type != null)
                        break;
                }

            }
            return type;
        });
    }

    protected abstract IEnumerable<Assembly> GetAssemblies();
}
