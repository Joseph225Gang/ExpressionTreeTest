using ExpressionTreeTest;
using System.Linq.Expressions;

var list  = new List<int>().AsQueryable().Where(n => n %2 == 0);

var myClass = new MyClass();

var id = 42;

Func<MyClass, string> func = c => c.MyMethod(id, "My Tested ASP.NET");
Func<MyClass, bool> propFunc = c => c.MyProperty;
Func<int, int, int> sum = (x , y) => x + y;

id = 42;

Console.WriteLine(func.Method.Name);
Expression<Func<MyClass, string>> expr = c => c.MyMethod(id, "My Tested ASP.NET");
Expression<Func<MyClass, string>> expr1 = c => c.MyMethod(42, "My Tested ASP.NET");
Expression<Func<MyClass, bool>> propExpr = c => c.MyProperty;
Expression<Func<MyClass, string>> expr2 = c => c.MyMethod(id, "My Tested ASP.NET");

var numberConstant = Expression.Constant(42);
var textConstant = Expression.Constant("My Tested ASP.NET");

var myClassType = typeof(MyClass);

var parameterExpression = Expression.Parameter(typeof(MyClass), "c");

var methodInfo = myClassType.GetMethod(nameof(MyClass.MyMethod));

var callExpression = Expression.Call(parameterExpression, methodInfo, numberConstant, textConstant);

var lambdaExpression = Expression.Lambda<Func<MyClass, string>>(callExpression, parameterExpression);

var func1 = lambdaExpression.Compile();

Console.WriteLine(func1(myClass));


/*ParseExpression(expr);
ParseExpression(expr1);
ParseExpression(propExpr);*/
Console.WriteLine(expr.ToString());
Console.WriteLine(expr2.ToString());
var exprFunc = expr.Compile();
var result = exprFunc(myClass);
Console.WriteLine(result);

static void AnotherMethod(Func<MyClass, string> someFunc)
{

}

static void ParseExpression(Expression expression)
{
    if(expression.NodeType == ExpressionType.Lambda)
    {
        var lambdaExpression = (LambdaExpression)expression;

        Console.Write("Lambda ");

        Console.WriteLine(lambdaExpression.Parameters[0].Name);

        ParseExpression(lambdaExpression.Body); 

    }
    else if(expression.NodeType == ExpressionType.Call)
    {
        var methodCallExpression = (MethodCallExpression)expression;

        Console.Write("Method ");

        Console.WriteLine(methodCallExpression.Method.Name);

        for (int i = 0; i < methodCallExpression.Arguments.Count; i++)
        {
            ParseExpression(methodCallExpression.Arguments[i]);
        }
    }
    else if(expression.NodeType == ExpressionType.MemberAccess)
    {
        var memberExpression = (MemberExpression)expression;

        Console.Write("Property ");

        Console.WriteLine(memberExpression.Member.Name);
    }
    else if(expression.NodeType == ExpressionType.Constant)
    {
        var constantExpression = (ConstantExpression)expression;

        Console.Write("Constant ");

        Console.WriteLine(constantExpression.Value);
    }
}
