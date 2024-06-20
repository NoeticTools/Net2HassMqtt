namespace NoeticTools.Net2HassMqtt.Configuration;

public sealed class EntityCategory
{
    private EntityCategory(string? hassEntityCategoryName)
    {
        HassEntityCategoryName = hassEntityCategoryName;
    }

    public static EntityCategory Configuration { get; } = new("config");

    public static EntityCategory Diagnostic { get; } = new("diagnostic");

    public string? HassEntityCategoryName { get; }

    public static EntityCategory None { get; } = new(null);
}