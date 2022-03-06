//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using Assimalign.Extensions.Linq.Serialization.Factories;

//using Assimalign.Extensions.Linq.Serialization.Internals;
//using Assimalign.Extensions.Linq.Serialization.Nodes;

//namespace Assimalign.Extensions.Linq.Serialization.Serializers;

//public class ExpressionConverter
//{
//    private readonly ExpressionCompressor _expressionCompressor;

//    public ExpressionConverter()
//    {
//        _expressionCompressor = new ExpressionCompressor();
//    }

//    public LinqExpressionNode Convert(Expression expression, LinqExpressionNodeFactorySettings factorySettings = null)
//    {
//        expression = _expressionCompressor.Compress(expression);

//        var factory = CreateFactory(expression, factorySettings);
//        return factory.Create(expression);
//    }

//    protected virtual ILinqExpressionNodeFactory CreateFactory(Expression expression, LinqExpressionNodeFactorySettings factorySettings)
//    {
//        if (expression is LambdaExpression lambda)
//            return new LinqExpressionNodeFactoryDefault(lambda.Parameters.Select(p => p.Type), factorySettings);
//        return new LinqExpressionNodeFactory(factorySettings);
//    }
//}
