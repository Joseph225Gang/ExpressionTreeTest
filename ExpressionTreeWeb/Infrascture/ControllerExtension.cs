using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace ExpressionTreeWeb.Infrascture
{
    public static class ControllerExtension
    {
        private static readonly ConcurrentDictionary<string ,string> actionNameCache = new ConcurrentDictionary<string ,string>();
        public static IActionResult RedirectTo<TController>(
            this Controller controller,
            Expression<Action<TController>> redirectExpression
            )
        {
            if(redirectExpression.Body.NodeType != ExpressionType.Call)
            {
                throw new InvalidOperationException($"The provided expression is not valid");
            }

            var methodCallExpression = (MethodCallExpression)redirectExpression.Body;

            var actionName = GetActionName(methodCallExpression);
            var controllerName = typeof(TController).Name.Replace(nameof(Controller), string.Empty);

            var routeValues = ExtractRouteValues(methodCallExpression);

            return controller.RedirectToAction( controllerName, actionName, routeValues );
        }
        
        private static string GetActionName(MethodCallExpression expression)
        {
            var methodName = expression.Method.Name;

            var actionName = expression
                .Method
                .GetCustomAttributes(true)
                .OfType<ActionNameAttribute>()
                .FirstOrDefault()
                ?.Name;

            return actionName ?? methodName;
        }

        private static RouteValueDictionary ExtractRouteValues(MethodCallExpression expression)
        {
            var names = expression.Method
                      .GetParameters()
                      .Select(parameter => parameter.Name)
                      .ToArray();
            var values = expression.Arguments
                         .Select(arg =>
                         {
                             if(arg.NodeType == ExpressionType.Constant)
                             {
                                 var constantExpression = (ConstantExpression)arg;
                                 return constantExpression.Value;
                             }
                             var convertExpression = Expression.Convert(arg, typeof(string));
                             var funcExpression = Expression.Lambda<Func<object>>(convertExpression);
                             return funcExpression.Compile().Invoke();
                         })
                         .ToArray();
            var routeValueDictionary = new RouteValueDictionary();
            for (int i = 0; i < names.Length; i++)
            {
                routeValueDictionary.Add(names[i], values[i] );
            }
            return routeValueDictionary;
        }
    }
}
