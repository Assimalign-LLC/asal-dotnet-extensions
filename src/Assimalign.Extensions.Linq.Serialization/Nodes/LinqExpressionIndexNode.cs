using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "X")]
public class LinqExpressionIndexNode : LinqExpressionNode<IndexExpression>
{
    public LinqExpressionIndexNode() { }

    public LinqExpressionIndexNode(ILinqExpressionNodeFactory factory, IndexExpression expression)
        : base(factory, expression) { }


    [DataMember(EmitDefaultValue = false, Name = "A")]
    public LinqExpressionNodeList Arguments { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "I")]
    public LinqExpressionPropertyInfoNode Indexer { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "O")]
    public LinqExpressionNode Object { get; set; }

    protected override void Initialize(IndexExpression expression)
    {
        Arguments = new LinqExpressionNodeList(Factory, expression.Arguments);
        Indexer = new LinqExpressionPropertyInfoNode(Factory, expression.Indexer);
        Object = Factory.Create(expression.Object);
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        return Expression.MakeIndex(
            Object.ToExpression(context),
            Indexer.ToMemberInfo(context),
            Arguments.GetExpressions(context));
    }
}

