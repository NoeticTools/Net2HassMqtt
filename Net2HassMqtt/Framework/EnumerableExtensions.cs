namespace NoeticTools.Net2HassMqtt.Framework;

public static class EnumerableExtensions
{
    public static void Foreach<T>(this IEnumerable<T> items, Action<T> action)
    {
        foreach (var item in items)
        {
            action(item);
        }
    }

    public static async Task ForeachAsync<T>(this IEnumerable<T> items, Func<T, Task> action)
    {
        foreach (var item in items)
        {
            await action(item);
        }
    }
}