using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Sieve.Services;

namespace Sieve.Extensions
{
    public static partial class LinqExtensions
    {
        public static IQueryable<TEntity> OrderByDynamic<TEntity>(
            this IQueryable<TEntity> source,
            string fullPropertyName,
            PropertyInfo propertyInfo,
            bool desc,
            bool useThenBy, 
            bool disableNullableTypeExpression = false)
        {
            var lambda = GenerateLambdaWithSafeMemberAccess<TEntity>(fullPropertyName, Array.Empty<string>(), propertyInfo, null, disableNullableTypeExpression);

            var command = desc
                ? (useThenBy ? "ThenByDescending" : "OrderByDescending")
                : (useThenBy ? "ThenBy" : "OrderBy");

            var resultExpression = Expression.Call(
                typeof(Queryable),
                command,
                new Type[] { typeof(TEntity), lambda.ReturnType },
                source.Expression,
                Expression.Quote(lambda));

            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static IQueryable<TEntity> OrderByDynamic<TEntity>(
            this IQueryable<TEntity> source,
            string fullPropertyName,
            PropertyInfo propertyInfo,
            string[] nestedJsonProperties,
            bool desc,
            bool useThenBy, 
            ISieveJsonAccessor jsonAccessor,
            bool disableNullableTypeExpression = false)
        {
            var lambda = GenerateLambdaWithSafeMemberAccess<TEntity>(fullPropertyName, nestedJsonProperties, propertyInfo, jsonAccessor, disableNullableTypeExpression);

            var command = desc
                ? (useThenBy ? "ThenByDescending" : "OrderByDescending")
                : (useThenBy ? "ThenBy" : "OrderBy");

            var resultExpression = Expression.Call(
                typeof(Queryable),
                command,
                new Type[] { typeof(TEntity), lambda.ReturnType },
                source.Expression,
                Expression.Quote(lambda));

            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
        
        private static Expression<Func<TEntity, object>> GenerateLambdaWithSafeMemberAccess<TEntity>
        (
            string fullPropertyName,
            string[] nestedJsonProperties,
            PropertyInfo propertyInfo,
            ISieveJsonAccessor jsonAccessor,
            bool disableNullableTypeExpression
        )
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            Expression propertyValue = parameter;
            Expression nullCheck = null;

            foreach (var name in fullPropertyName.Split('.'))
            {
                try
                {
                    propertyValue = Expression.PropertyOrField(propertyValue, name);
                }
                catch (ArgumentException)
                {
                    // name is not a direct property of field of propertyValue expression. construct a memberAccess then.
                    propertyValue = Expression.MakeMemberAccess(propertyValue, propertyInfo);
                }

                if (propertyValue.Type.IsNullable() && !disableNullableTypeExpression)
                {
                    nullCheck = GenerateOrderNullCheckExpression(propertyValue, nullCheck);
                }
            }

            var expression = nullCheck == null
                ? propertyValue
                : Expression.Condition(nullCheck, Expression.Default(propertyValue.Type), propertyValue);

            if (nestedJsonProperties.Length != 0 && jsonAccessor != null)
            {
                expression = jsonAccessor.GetJsonPropertyExpression(expression, nestedJsonProperties, propertyInfo);
            }

            var converted = Expression.Convert(expression, typeof(object));
            return Expression.Lambda<Func<TEntity, object>>(converted, parameter);
        }

        private static Expression GenerateOrderNullCheckExpression(Expression propertyValue, Expression nullCheckExpression)
        {
            return nullCheckExpression == null
                ? Expression.Equal(propertyValue, Expression.Default(propertyValue.Type))
                : Expression.OrElse(nullCheckExpression, Expression.Equal(propertyValue, Expression.Default(propertyValue.Type)));
        }
    }
}
