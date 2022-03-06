using Assimalign.Extensions.Linq.Serialization;
using Assimalign.Extensions.Linq.Serialization.Nodes;
using System.Linq.Expressions;

Expression<Func<Test, string>> exp = x => x.FirstName;

var results = exp.ToJson();

Console.WriteLine(results);


var serializer = new LinqExpressionJsonSerializer();

var re = serializer.Deserialize<LinqExpressionLambdaNode>(results);
var ex = re.ToExpression();

if  (ex is Expression<Func<Test, string>> lamb)
{
	var t = new Test()
	{
		FirstName = "Chase"
	};
	var r = lamb.Compile().Invoke(t);
}

Console.WriteLine(re.ToString());


public class Test 
{
	public string FirstName { get; set; }
}






