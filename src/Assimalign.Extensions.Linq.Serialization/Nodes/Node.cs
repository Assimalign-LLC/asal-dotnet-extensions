using System;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

/// <summary>
/// 
/// </summary>
[DataContract]
[Serializable]
#region KnownTypes
[KnownType(typeof(LinqExpressionBinaryNode))]
[KnownType(typeof(LinqExpressionConditionalNode))]
[KnownType(typeof(LinqExpressionConstantNode))]
[KnownType(typeof(LinqExpressionConstructorInfoNode))]
[KnownType(typeof(LinqExpressionNodeDefault))]
[KnownType(typeof(LinqExpressionElementInitNode))]
[KnownType(typeof(LinqExpressionElementInitNodeList))]
[KnownType(typeof(LinqExpressionNode))]
[KnownType(typeof(LinqExpressionNodeList))]
[KnownType(typeof(LinqExpressionFieldInfoNode))]
[KnownType(typeof(LinqExpressionIndexNode))]
[KnownType(typeof(LinqExpressionInvocationNode))]
[KnownType(typeof(LinqExpressionLambdaNode))]
[KnownType(typeof(LinqExpressionListInitNode))]
[KnownType(typeof(LinqExpressionMemberAssignmentNode))]
[KnownType(typeof(LinqExpressionMemberBindingNode))]
[KnownType(typeof(LinqExpressionMemberBindingNodeList))]
[KnownType(typeof(LinqExpressionMemberNode))]
[KnownType(typeof(LinqExpressionMemberInfoNode))]
[KnownType(typeof(LinqExpressionMemberInfoNodeList))]    
[KnownType(typeof(LinqExpressionMemberInitNode))]
[KnownType(typeof(LinqExpressionMemberListBindingNode))]
[KnownType(typeof(LinqExpressionMemberMemberBindingNode))]
[KnownType(typeof(LinqExpressionMethodCallNode))]
[KnownType(typeof(LinqExpressionNewArrayNode))]
[KnownType(typeof(LinqExpressionNewNode))]
[KnownType(typeof(LinqExpressionParameterNode))]
[KnownType(typeof(LinqExpressionPropertyInfoNode))]    
[KnownType(typeof(LinqExpressionTypeBinaryNode))]
[KnownType(typeof(TypeNode))]
[KnownType(typeof(LinqExpressionUnaryNode))]
#endregion
public abstract class Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Node"/> class.
    /// </summary>
    protected Node() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="Node"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <exception cref="System.ArgumentNullException">factory</exception>
    protected Node(ILinqExpressionNodeFactory factory)
    {
        Factory = factory ?? throw new ArgumentNullException("factory");
    }

    /// <summary>
    /// Gets the factory.
    /// </summary>
    /// <value>
    /// The factory.
    /// </value>
    [NonSerialized]
    [IgnoreDataMember]
    public readonly ILinqExpressionNodeFactory Factory;        
}
