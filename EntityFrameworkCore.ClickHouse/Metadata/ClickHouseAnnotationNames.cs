using System;

namespace ClickHouse.EntityFrameworkCore.Metadata;
public static class ClickHouseAnnotationNames
{
    public const string Prefix = "ClickHouse:";

    public const string Engine = Prefix + "Engine";
}
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class ClickHouseTableAttribute : Attribute
{
    public ClickHouseTableAttribute(TableCreationStrategy strategy, ImplicitNavigationStrategy navStrategy = ImplicitNavigationStrategy.IGNORE)
    {
        Strategy = strategy;
        NavStrategy = navStrategy;
    }

    public TableCreationStrategy Strategy { get; }
    public ImplicitNavigationStrategy NavStrategy { get; set; }
}
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ClickHouseIgnore: Attribute
{

}
public enum ImplicitNavigationStrategy
{
    IGNORE = 1,
    CREATE_AS_DEFAULT_ENGINE
}
public enum TableCreationStrategy
{
    CREATE = 1,
    CREATE_IF_NOT_EXISTS,
    CREATE_OR_REPLACE
}