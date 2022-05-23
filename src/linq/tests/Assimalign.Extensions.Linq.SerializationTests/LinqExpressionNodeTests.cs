using Xunit;
using System;
using System.Linq.Expressions;

namespace Assimalign.Extensions.Linq.SerializationTests;

using Assimalign.Extensions.Linq.Serialization;
using Assimalign.Extensions.Linq.Serialization.Factories;
using Assimalign.Extensions.Linq.SerializationTests.Internal;

public class LinqExpressionNodeTests
{
   


    /* Convert Then Revert Test
     * 
     * The expression should be converted to an internal 
     * node which represents the expression 
     */
    [Fact]
    public void ConvertThenRevertIntMathExpressionTest()
    {
        XunitUtilties.AssertExpression(Expression.Add(Expression.Constant(5), Expression.Constant(10)));
        XunitUtilties.AssertExpression(Expression.Subtract(Expression.Constant(5), Expression.Constant(10)));
        XunitUtilties.AssertExpression(Expression.Multiply(Expression.Constant(5), Expression.Constant(10)));
        XunitUtilties.AssertExpression(Expression.Divide(Expression.Constant(5), Expression.Constant(10)));
        XunitUtilties.AssertExpression(Expression.AddAssign(Expression.Variable(typeof(int), "x"), Expression.Constant(10)));
        XunitUtilties.AssertExpression(Expression.SubtractAssign(Expression.Variable(typeof(int), "x"), Expression.Constant(10)));
        XunitUtilties.AssertExpression(Expression.MultiplyAssign(Expression.Variable(typeof(int), "x"), Expression.Constant(10)));
        XunitUtilties.AssertExpression(Expression.DivideAssign(Expression.Variable(typeof(int), "x"), Expression.Constant(10)));
    }

    [Fact]
    public void ConvertThenRevertConditionalExpressionTest()
    {
        XunitUtilties.AssertExpression(Expression.Condition(Expression.Constant(true), Expression.Constant(5), Expression.Constant(10)));
        XunitUtilties.AssertExpression(Expression.Condition(Expression.Constant(false), Expression.Constant(5), Expression.Constant(10)));
    }

    [Fact]
    public void SimpleConditionalWithNullConstantTest()
    {
        var argParam = Expression.Parameter(typeof(Type), "type");
        var stringProperty = Expression.Property(argParam, "AssemblyQualifiedName");

        XunitUtilties.AssertExpression(Expression.Condition(Expression.Constant(true), stringProperty, Expression.Constant(null, typeof(string))));
    }

    [Fact]
    public void SimpleUnaryTest()
    {
        XunitUtilties.AssertExpression(Expression.UnaryPlus(Expression.Constant(43)));
    }

    [Fact]
    public void SimpleTypedNullConstantTest()
    {
        XunitUtilties.AssertExpression(Expression.Constant(null, typeof(string)));
    }

    [Fact]
    public void SimpleLambdaTest()
    {
        XunitUtilties.AssertExpression(Expression.Lambda(Expression.Constant("body"), Expression.Parameter(typeof(string))));
    }

    [Fact]
    public void SimpleTypeBinaryTest()
    {
        XunitUtilties.AssertExpression(Expression.TypeIs(Expression.Variable(GetType()), typeof(object)));
        XunitUtilties.AssertExpression(Expression.TypeEqual(Expression.Variable(GetType()), typeof(object)));
    }

    [Fact]
    public void SimpleMemberTest()
    {
        var type = typeof(Foo);
        var property = type.GetProperty("Name");

        var parameter = Expression.Parameter(type, "p");
        var propertyAccess = Expression.MakeMemberAccess(parameter, property);

        XunitUtilties.AssertExpression(propertyAccess);
    }
}