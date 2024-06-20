namespace NoeticTools.Net2HassMqtt.Framework;

public static class UniqueIdGenerator
{
    private static readonly Random Random = new();

    public static string GetNextInt64()
    {
        return $"0x{Random.NextInt64():x}";
    }
}