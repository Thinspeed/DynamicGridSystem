using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Sieve.Services;

namespace GridSystem.Api.SievePreferences;

public class SieveJsonAccessor : ISieveJsonAccessor
{
    public Expression GetJsonPropertyExpression(Expression expression, string[] nestedJsonProperties, PropertyInfo propertyInfo)
    {
        if (propertyInfo.PropertyType != typeof(JsonDocument))
        {
            throw new ArgumentException("Only JsonDocument properties are supported");
        }
        
        PropertyInfo rootElementProperty = typeof(JsonDocument).GetProperty(nameof(JsonDocument.RootElement))!;
        MethodInfo getPropertyMethod = typeof(JsonElement).GetMethod(nameof(JsonElement.GetProperty), new[] { typeof(string) })!;
        
        expression = Expression.Property(expression, rootElementProperty);

        foreach (string propertyName in nestedJsonProperties)
        {
            expression = Expression.Call(expression, getPropertyMethod, Expression.Constant(propertyName));
        }

        return expression;
    }
}