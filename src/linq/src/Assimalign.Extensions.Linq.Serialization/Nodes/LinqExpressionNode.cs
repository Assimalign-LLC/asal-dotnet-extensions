using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;


[Serializable]
[DataContract(Name = "E")]
public abstract class LinqExpressionNode : Node
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinqExpressionNode"/> class.
    /// </summary>
    protected LinqExpressionNode() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinqExpressionNode"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="nodeType">Type of the node.</param>
    /// <param name="type">The type.</param>
    protected LinqExpressionNode(ILinqExpressionNodeFactory factory, ExpressionType nodeType, Type type = null)
        : base(factory)
    {
        NodeType = nodeType;
        Type = new TypeNode(factory, type);
    }

    /// <summary>
    /// Gets or sets the type of the node.
    /// </summary>
    /// <value>
    /// The type of the node.
    /// </value>
    [DataMember(EmitDefaultValue = false, Name = "NT")]
    public ExpressionType NodeType { get; set; }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    /// <value>
    /// The type.
    /// </value>
    [DataMember(EmitDefaultValue = false, Name = "T")]
    public virtual TypeNode Type { get; set; }

    /// <summary>
    /// Converts this instance to an expression.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public virtual Expression ToExpression(ILinqExpressionContext context)
    {
        return null;
    }

    public Expression ToExpression()
    {
        return ToExpression(new LinqExpressionContext());
    }

    /// <summary>
    /// Converts this instance to an expression.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public Expression<TDelegate> ToExpression<TDelegate>(ILinqExpressionContext context = null)
    {
        return ToExpression(ConvertToExpression<TDelegate>, context ?? new LinqExpressionContext());
    }

    /// <summary>
    /// Converts this instance to an boolean expression.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    public Expression<Func<TEntity, bool>> ToBooleanExpression<TEntity>(ILinqExpressionContext context = null)
    {
        return ToExpression(ConvertToBooleanExpression<TEntity>, context ?? new LinqExpressionContext());
    }

    /// <summary>
    /// Converts this instance to an expression.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    /// <param name="conversionFunction">The conversion function.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">conversionFunction</exception>
    public Expression<TDelegate> ToExpression<TDelegate>(Func<LinqExpressionNode, Expression<TDelegate>> conversionFunction)
    {
        if (conversionFunction == null)
            throw new ArgumentNullException(nameof(conversionFunction));
        return conversionFunction(this);
    }

    /// <summary>
    /// Converts this instance to an expression.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    /// <param name="conversionFunction">The conversion function.</param>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">
    /// Parameter <paramref name="conversionFunction"/> or <paramref name="context"/> is null.
    /// </exception>
    public Expression<TDelegate> ToExpression<TDelegate>(Func<LinqExpressionNode, ILinqExpressionContext, Expression<TDelegate>> conversionFunction, ILinqExpressionContext context)
    {
        if (conversionFunction == null)
            throw new ArgumentNullException(nameof(conversionFunction));
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        return conversionFunction(this, context);
    }

    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
        return ToExpression().ToString();
    }

    /// <summary>
    /// Converts to an expression.
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    /// <param name="expressionNode">The expression node.</param>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    private static Expression<TDelegate> ConvertToExpression<TDelegate>(LinqExpressionNode expressionNode, ILinqExpressionContext context)
    {
        var expression = expressionNode.ToExpression(context);
        return (Expression<TDelegate>)expression;
    }

    /// <summary>
    /// Converts to a boolean expression.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="expressionNode">The expression node.</param>
    /// <param name="context">The context.</param>
    /// <returns></returns>
    private static Expression<Func<TEntity, bool>> ConvertToBooleanExpression<TEntity>(LinqExpressionNode expressionNode, ILinqExpressionContext context)
    {
        return ConvertToExpression<Func<TEntity, bool>>(expressionNode, context);
    }
}


[Serializable]
[DataContract(Name = "tE")]    
[DebuggerDisplay("ExpressionNode")]
public abstract class LinqExpressionNode<TExpression> : LinqExpressionNode 
    where TExpression : Expression
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinqExpressionNode{TExpression}"/> class.
    /// </summary>
    protected LinqExpressionNode() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinqExpressionNode{TExpression}"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="expression">The expression.</param>
    protected LinqExpressionNode(ILinqExpressionNodeFactory factory, TExpression expression)
        : base(factory, expression.NodeType, expression.Type)
    {
        Initialize(expression);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinqExpressionNode{TExpression}"/> class.
    /// </summary>
    /// <param name="factory">The factory.</param>
    /// <param name="nodeType">Type of the node.</param>
    /// <param name="type">The type.</param>
    protected LinqExpressionNode(ILinqExpressionNodeFactory factory, ExpressionType nodeType, Type type = null)
        : base(factory, nodeType, type) { }

    /// <summary>
    /// Initializes this instance using the specified expression.
    /// </summary>
    /// <param name="expression">The expression.</param>
    protected abstract void Initialize(TExpression expression);
}
