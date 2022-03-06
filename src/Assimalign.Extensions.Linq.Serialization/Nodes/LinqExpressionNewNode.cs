using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;


namespace Assimalign.Extensions.Linq.Serialization.Nodes;

[Serializable]
[DataContract(Name = "N")]
public class LinqExpressionNewNode : LinqExpressionNode<NewExpression>
{
    public LinqExpressionNewNode() { }

    public LinqExpressionNewNode(ILinqExpressionNodeFactory factory, NewExpression expression)
        : base(factory, expression) { }


    [DataMember(EmitDefaultValue = false, Name = "A")]
    public LinqExpressionNodeList Arguments { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "C")]
    public LinqExpressionConstructorInfoNode Constructor { get; set; }


    [DataMember(EmitDefaultValue = false, Name = "M")]
    public LinqExpressionMemberInfoNodeList Members { get; set; }

    protected override void Initialize(NewExpression expression)
    {
        Arguments = new LinqExpressionNodeList(Factory, expression.Arguments);
        Constructor = new LinqExpressionConstructorInfoNode(Factory, expression.Constructor);
        Members = new LinqExpressionMemberInfoNodeList(Factory, expression.Members);
    }

    public override Expression ToExpression(ILinqExpressionContext context)
    {
        if (Constructor == null)
            return Expression.New(Type.ToType(context));

        var constructor = Constructor.ToMemberInfo(context);
        if (constructor == null)
            return Expression.New(Type.ToType(context));

        var arguments = Arguments.GetExpressions(context).ToArray();
        var members = Members != null ? Members.GetMembers(context).ToArray() : null;
        return members != null && members.Length > 0 ? Expression.New(constructor, arguments, members) : Expression.New(constructor, arguments);
    }
}
