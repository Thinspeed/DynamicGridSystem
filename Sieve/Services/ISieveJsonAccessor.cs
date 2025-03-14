using System.Linq.Expressions;
using System.Reflection;

namespace Sieve.Services
{
    public interface ISieveJsonAccessor
    {
        Expression GetJsonPropertyExpression(Expression expression, string[] nestedJsonProperties, PropertyInfo propertyInfo);
    }
}