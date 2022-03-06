
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using Assimalign.Extensions.Linq.Serialization.Extensions;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;



[Serializable]
[DataContract(Name = "L")]
public class LinqExpressionLambdaNode : LinqExpressionNode<LambdaExpression>
{
    public LinqExpressionLambdaNode() { }

    public LinqExpressionLambdaNode(ILinqExpressionNodeFactory factory, LambdaExpression expression)
        : base(factory, expression) { }


    [DataMember(EmitDefaultValue = false, Name = "B")]
    public LinqExpressionNode Body { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "P")]
    public LinqExpressionNodeList Parameters { get; set; }

    protected override void Initialize(LambdaExpression expression)
    {
        Parameters = new LinqExpressionNodeList(Factory, expression.Parameters);
        Body = Factory.Create(expression.Body);
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        var body = Body.ToExpression(context);
        var parameters = Parameters.GetParameterExpressions(context).ToArray();

        var bodyParameters = body.GetNodes().OfType<ParameterExpression>().ToArray();
        for (var i = 0; i < parameters.Length; ++i)
        {
            var matchingParameter = bodyParameters.Where(p => p.Name == parameters[i].Name && p.Type == parameters[i].Type).ToArray();
            if (matchingParameter.Length == 1)
                parameters[i] = matchingParameter.First();
        }

        return Expression.Lambda(Type.ToType(context), body, parameters);
    }
}
