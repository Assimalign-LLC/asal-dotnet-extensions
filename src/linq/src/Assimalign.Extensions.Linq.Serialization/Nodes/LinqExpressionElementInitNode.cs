using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "EI")]
public class LinqExpressionElementInitNode : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinqExpressionElementInitNode"/> class.
    /// </summary>
    public LinqExpressionElementInitNode() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinqExpressionElementInitNode"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="elementInit">The element init.</param>
    public LinqExpressionElementInitNode(ILinqExpressionNodeFactory factory, ElementInit elementInit)
        : base(factory)
    {
        Initialize(elementInit);
    }

    /// <summary>
    /// Initializes the specified element init.
    /// </summary>
    /// <param name="elementInit">The element init.</param>
    /// <exception cref="System.ArgumentNullException">elementInit</exception>
    private void Initialize(ElementInit elementInit)
    {
        if (elementInit == null)
            throw new ArgumentNullException("elementInit");

        AddMethod = new LinqExpressionMethodInfoNode(Factory, elementInit.AddMethod);
        Arguments = new LinqExpressionNodeList(Factory, elementInit.Arguments);
    }

    /// <summary>
    /// Gets or sets the arguments.
    /// </summary>
    /// <value>
    /// The arguments.
    /// </value>
    [DataMember(EmitDefaultValue = false, Name = "A")]
    public LinqExpressionNodeList Arguments { get; set; }

    /// <summary>
    /// Gets or sets the add method.
    /// </summary>
    /// <value>
    /// The add method.
    /// </value>
    [DataMember(EmitDefaultValue = false, Name = "M")]
    public LinqExpressionMethodInfoNode AddMethod { get; set; }

    internal ElementInit ToElementInit(ILinqExpressionContext context)
    {
        return Expression.ElementInit(AddMethod.ToMemberInfo(context), Arguments.GetExpressions(context));
    }
}
