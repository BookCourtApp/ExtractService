using System.Collections.Concurrent;

namespace Core;

public static class Extension
{
    public static async Task<IEnumerable<T>> AsyncParallelWhereOrderedByCompletion<T>(
        this IEnumerable<T> source, Func<T, Task<bool>> predicate)
    {
        var results = new ConcurrentQueue<T>();
        var tasks = source.Select(
            async x =>
            {
                if (await predicate(x))
                    results.Enqueue(x);
            });
        await Task.WhenAll(tasks);
        return results;
    }
}