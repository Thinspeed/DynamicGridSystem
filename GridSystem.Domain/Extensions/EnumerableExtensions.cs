namespace GridSystem.Domain.Extensions;

public static class EnumerableExtensions
{
    public static T FirstOrThrow<T, TException>(this IEnumerable<T> collection, Func<T, bool> predicate,
        Func<TException> exceptionFactory)
        where TException : Exception
    {
        var result = collection.FirstOrDefault(predicate);
        if (result == null)
        {
            throw exceptionFactory();
        }

        return result;
    }
}