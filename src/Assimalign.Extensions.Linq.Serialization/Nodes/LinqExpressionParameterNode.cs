
using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "P")]
public class LinqExpressionParameterNode : LinqExpressionNode<ParameterExpression>
{
    public LinqExpressionParameterNode() { }

    public LinqExpressionParameterNode(ILinqExpressionNodeFactory factory, ParameterExpression expression)
        : base(factory, expression) { }


    [DataMember(EmitDefaultValue = false, Name = "I")]
    public bool IsByRef { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "N")]
    public string Name { get; set; }

    protected override void Initialize(ParameterExpression expression)
    {
        IsByRef = expression.IsByRef;
        Name = expression.Name;
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        return context.GetParameterExpression(this);
    }
}
