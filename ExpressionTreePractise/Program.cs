using ExpressionTreePractise;
using System.Linq.Expressions;

Func<ETClass, string> func = c => c.ETMethod(20, "love");
Expression<Func<ETClass, string>> expre = c => c.ETMethod(20, "My Tested ASP.NET");

var numberConstant = Expression.Constant(42);
var textConstant = Expression.Constant("My Tested ASP.NET");

Expression callExpr = Expression.Call(
      Expression.New(typeof(SampleClass)),
      typeof(SampleClass).GetMethod("AddIntegers", new Type[] { typeof(int), typeof(int) }),
      Expression.Constant(1),
      Expression.Constant(2)
      );


// Print out the expression.
Console.WriteLine(callExpr.ToString());

// The following statement first creates an expression tree,
// then compiles it, and then executes it.
Console.WriteLine(Expression.Lambda<Func<int>>(callExpr).Compile()());

// This code example produces the following output:
//
// new SampleClass().AddIntegers(1, 2)
// 3

Expression pracExpr = Expression.Call(
            Expression.New(typeof(ETClass)),
            typeof(ETClass).GetMethod("ETMethod", new Type[] { typeof(int), typeof(string) }),
            Expression.Constant(2),
            Expression.Constant("She")
    );
Console.WriteLine(pracExpr.ToString());

Console.WriteLine(Expression.Lambda<Func<string>>(pracExpr).Compile()());


public class SampleClass
{
    public int AddIntegers(int arg1, int arg2)
    {
        return arg1 + arg2;
    }
}