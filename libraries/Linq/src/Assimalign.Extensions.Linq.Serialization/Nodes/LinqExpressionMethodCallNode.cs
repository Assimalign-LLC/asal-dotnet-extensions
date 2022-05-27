using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "MC")]
public class LinqExpressionMethodCallNode : LinqExpressionNode<MethodCallExpression>
{
    public LinqExpressionMethodCallNode() { }

    public LinqExpressionMethodCallNode(ILinqExpressionNodeFactory factory, MethodCallExpression expression)
        : base(factory, expression) { }


    [DataMember(EmitDefaultValue = false, Name = "A")]
    public LinqExpressionNodeList Arguments { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "M")]
    public LinqExpressionMethodInfoNode Method { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "O")]
    public LinqExpressionNode Object { get; set; }

    protected override void Initialize(MethodCallExpression expression)
    {
        Arguments = new LinqExpressionNodeList(Factory, expression.Arguments);
        Method = new LinqExpressionMethodInfoNode(Factory, expression.Method);
        Object = Factory.Create(expression.Object);
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        Expression objectExpression = null;
        if (Object != null)
            objectExpression = Object.ToExpression(context);

        return Expression.Call(objectExpression, Method.ToMemberInfo(context), Arguments.GetExpressions(context).ToArray());
    }
}
