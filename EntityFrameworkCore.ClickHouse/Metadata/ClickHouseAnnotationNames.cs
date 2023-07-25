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
    public ClickHouseTableAttribute(TableCreationStrategy strategy)
    {
        Strategy = strategy;
    }

    public TableCreationStrategy Strategy { get; }
}
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class ClickHouseIgnore: Attribute
{

}

