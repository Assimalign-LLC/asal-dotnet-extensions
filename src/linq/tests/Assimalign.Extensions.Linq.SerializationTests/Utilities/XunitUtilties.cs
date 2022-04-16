using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace Assimalign.Extensions.Linq.SerializationTests;

using Assimalign.Extensions.Linq.Serialization;
using Assimalign.Extensions.Linq.Serialization.Factories;

internal static class XunitUtilties
{

    public static void AssertExpression(Expression expression, string message = null)
    {
        AssertExpression<LinqExpressionNodeFactory>(expression, message);
    }

    private static void AssertExpression<TFactory>(Expression expression, string message = null)
        where TFactory : ILinqExpressionNodeFactory
    {
        var factory = (TFactory)Activator.CreateInstance(typeof(TFactory));
        var expressionNode = factory.Create(expression);
        var createdExpression = expressionNode.ToExpression();

        XunitUtilties.AreEqual(expression, createdExpression, message);

        //PublicInstancePropertiesAssert.AreEqual(expression, createdExpression, message);
        //Assert.AreEqual(expression.ToString(), createdExpression.ToString(), message);

        Console.WriteLine("'{0}' == '{1}'", expression.ToString(), createdExpression.ToString());
    }


    public static void AreEqual<TDelegate>( Expression<TDelegate> expected, Expression<TDelegate> actual, string message = null)
        where TDelegate : Delegate
    {
        AreEqual(expected, (Expression)actual, message);
    }

    public static void AreEqual( Expression expected, Expression actual, string message = null)
    {

        var result = AreEqual(expected, actual);

        var failMessage = !string.IsNullOrWhiteSpace(message) ? message : string.Empty;
        failMessage += $"Expected was <{expected}>, Actual was <{actual}>";

        Assert.True(result, failMessage);
    }

    public static bool AreEqual(Expression x, Expression y)
    {
        if (ReferenceEquals(x, y))
            return true;
        if (x.NodeType != y.NodeType)
            return false;

        switch (x.NodeType)
        {
            case ExpressionType.Negate:
            case ExpressionType.NegateChecked:
            case ExpressionType.Not:
            case ExpressionType.Convert:
            case ExpressionType.ConvertChecked:
            case ExpressionType.ArrayLength:
            case ExpressionType.Quote:
            case ExpressionType.TypeAs:
            case ExpressionType.UnaryPlus:
                return AreEqualUnary((UnaryExpression)x, (UnaryExpression)y);
            case ExpressionType.Add:
            case ExpressionType.AddAssign:
            case ExpressionType.AddAssignChecked:
            case ExpressionType.AddChecked:
            case ExpressionType.Assign:
            case ExpressionType.Subtract:
            case ExpressionType.SubtractAssign:
            case ExpressionType.SubtractAssignChecked:
            case ExpressionType.SubtractChecked:
            case ExpressionType.Multiply:
            case ExpressionType.MultiplyAssign:
            case ExpressionType.MultiplyAssignChecked:
            case ExpressionType.MultiplyChecked:
            case ExpressionType.Divide:
            case ExpressionType.DivideAssign:
            case ExpressionType.Modulo:
            case ExpressionType.ModuloAssign:
            case ExpressionType.And:
            case ExpressionType.AndAlso:
            case ExpressionType.Or:
            case ExpressionType.OrElse:
            case ExpressionType.LessThan:
            case ExpressionType.LessThanOrEqual:
            case ExpressionType.GreaterThan:
            case ExpressionType.GreaterThanOrEqual:
            case ExpressionType.Equal:
            case ExpressionType.NotEqual:
            case ExpressionType.Coalesce:
            case ExpressionType.ArrayIndex:
            case ExpressionType.RightShift:
            case ExpressionType.LeftShift:
            case ExpressionType.ExclusiveOr:
                return AreEqualBinary((BinaryExpression)x, (BinaryExpression)y);
            case ExpressionType.TypeIs:
            case ExpressionType.TypeEqual:
                return AreEqualTypeBinary((TypeBinaryExpression)x, (TypeBinaryExpression)y);
            case ExpressionType.Conditional:
                return AreEqualConditional((ConditionalExpression)x, (ConditionalExpression)y);
            case ExpressionType.Constant:
                return AreEqualConstant((ConstantExpression)x, (ConstantExpression)y);
            case ExpressionType.Parameter:
                return AreEqualParameter((ParameterExpression)x, (ParameterExpression)y);
            case ExpressionType.MemberAccess:
                return AreEqualMemberAccess((MemberExpression)x, (MemberExpression)y);
            case ExpressionType.Call:
                return AreEqualMethodCall((MethodCallExpression)x, (MethodCallExpression)y);
            case ExpressionType.Lambda:
                return AreEqualLambda((LambdaExpression)x, (LambdaExpression)y);
            case ExpressionType.New:
                return AreEqualNew((NewExpression)x, (NewExpression)y);
            case ExpressionType.NewArrayInit:
            case ExpressionType.NewArrayBounds:
                return AreEqualNewArray((NewArrayExpression)x, (NewArrayExpression)y);
            case ExpressionType.Invoke:
                return AreEqualInvocation((InvocationExpression)x, (InvocationExpression)y);
            case ExpressionType.MemberInit:
                return AreEqualMemberInit((MemberInitExpression)x, (MemberInitExpression)y);
            case ExpressionType.ListInit:
                return AreEqualListInit((ListInitExpression)x, (ListInitExpression)y);
            case ExpressionType.Default:
                return AreEqualDefault((DefaultExpression)x, (DefaultExpression)y);
            default:
                throw new Exception($"Unhandled expression type: '{x.NodeType}'");
        }
    }

    public static bool AreEqualBinding(MemberBinding x, MemberBinding y)
    {
        if (x.BindingType != y.BindingType)
            return false;

        switch (x.BindingType)
        {
            case MemberBindingType.Assignment:
                return AreEqualMemberAssignment((MemberAssignment)x, (MemberAssignment)y);
            case MemberBindingType.MemberBinding:
                return AreEqualMemberMemberBinding((MemberMemberBinding)x, (MemberMemberBinding)y);
            case MemberBindingType.ListBinding:
                return AreEqualMemberListBinding((MemberListBinding)x, (MemberListBinding)y);
            default:
                throw new Exception($"Unhandled binding type '{y.BindingType}'");
        }
    }

    public static bool AreEqualElementInitializer(ElementInit x, ElementInit y)
    {
        return AreEqualExpressionList(x.Arguments, y.Arguments);
    }

    public static bool AreEqualUnary(UnaryExpression x, UnaryExpression y)
    {
        return AreEqual(x.Operand, y.Operand);
    }

    public static bool AreEqualBinary(BinaryExpression x, BinaryExpression y)
    {
        return AreEqual(x.Left, y.Left)
            && AreEqual(x.Right, y.Right)
            && AreEqual(x.Conversion, y.Conversion);
    }

    public static bool AreEqualTypeBinary(TypeBinaryExpression x, TypeBinaryExpression y)
    {
        return x.NodeType == y.NodeType
            && x.TypeOperand == y.TypeOperand
            && AreEqual(x.Expression, y.Expression);
    }

    public static bool AreEqualConstant(ConstantExpression x, ConstantExpression y)
    {
        return x.Type == y.Type
            && (ReferenceEquals(x.Value, y.Value) || x.Value.Equals(y.Value));
    }

    public static bool AreEqualConditional(ConditionalExpression x, ConditionalExpression y)
    {
        return AreEqual(x.Test, y.Test)
            && AreEqual(x.IfTrue, y.IfTrue)
            && AreEqual(x.IfFalse, y.IfFalse);
    }

    public static bool AreEqualDefault(DefaultExpression x, DefaultExpression y)
    {
        return x.Type == y.Type;
    }

    public static bool AreEqualParameter(ParameterExpression x, ParameterExpression y)
    {
        return x.Type == y.Type
            && (ReferenceEquals(x.Name, y.Name) || x.Name.Equals(y.Name));
    }

    public static bool AreEqualMemberAccess(MemberExpression x, MemberExpression y)
    {
        return AreEqual(x.Expression, y.Expression);
    }

    public static bool AreEqualMethodCall(MethodCallExpression x, MethodCallExpression y)
    {
        var isEqual = AreEqual(x.Object, y.Object);
        if (isEqual) isEqual = AreEqualExpressionList(x.Arguments, y.Arguments);
        return isEqual;
    }

    public static bool AreEqualExpressionList(ReadOnlyCollection<Expression> x, ReadOnlyCollection<Expression> y)
    {
        var isEqual = x.Count.Equals(y.Count);
        for (var i = 0; isEqual && i < x.Count; ++i)
            isEqual = AreEqual(x[i], y[i]);
        return isEqual;
    }

    public static bool AreEqualMemberAssignment(MemberAssignment x, MemberAssignment y)
    {
        return AreEqual(x.Expression, y.Expression);
    }

    public static bool AreEqualMemberMemberBinding(MemberMemberBinding x, MemberMemberBinding y)
    {
        return AreEqualBindingList(x.Bindings, y.Bindings);
    }

    public static bool AreEqualMemberListBinding(MemberListBinding x, MemberListBinding y)
    {
        return AreEqualElementInitializerList(x.Initializers, y.Initializers);
    }

    public static bool AreEqualBindingList(ReadOnlyCollection<MemberBinding> x, ReadOnlyCollection<MemberBinding> y)
    {
        var isEqual = x.Count.Equals(y.Count);
        for (var i = 0; isEqual && i < x.Count; ++i)
            isEqual = AreEqualBinding(x[i], y[i]);
        return isEqual;
    }

    public static bool AreEqualElementInitializerList(ReadOnlyCollection<ElementInit> x, ReadOnlyCollection<ElementInit> y)
    {
        var isEqual = x.Count.Equals(y.Count);
        for (var i = 0; isEqual && i < x.Count; ++i)
            isEqual = AreEqualElementInitializer(x[i], y[i]);
        return isEqual;
    }

    public static bool AreEqualLambda(LambdaExpression x, LambdaExpression y)
    {
        return AreEqual(x.Body, y.Body);
    }

    public static bool AreEqualNew(NewExpression x, NewExpression y)
    {
        return AreEqualExpressionList(x.Arguments, y.Arguments);
    }

    public static bool AreEqualMemberInit(MemberInitExpression x, MemberInitExpression y)
    {
        return AreEqualNew(x.NewExpression, y.NewExpression)
            && AreEqualBindingList(x.Bindings, y.Bindings);
    }

    public static bool AreEqualListInit(ListInitExpression x, ListInitExpression y)
    {
        return AreEqualNew(x.NewExpression, y.NewExpression)
            && AreEqualElementInitializerList(x.Initializers, y.Initializers);
    }

    public static bool AreEqualNewArray(NewArrayExpression x, NewArrayExpression y)
    {
        return AreEqualExpressionList(x.Expressions, y.Expressions);
    }

    public static bool AreEqualInvocation(InvocationExpression x, InvocationExpression y)
    {
        return AreEqualExpressionList(x.Arguments, y.Arguments)
            && AreEqual(x.Expression, y.Expression);
    }
}
