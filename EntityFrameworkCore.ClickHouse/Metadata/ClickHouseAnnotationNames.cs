using System;

namespace ClickHouse.EntityFrameworkCore.Metadata;
public static class ClickHouseAnnotationNames
{
    public const string Prefix = "ClickHouse:";

    public const string Engine = Prefix + "Engine";
}
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class ClickHouseTableCreationStrategyAttribute : Attribute
{
    public ClickHouseTableCreationStrategyAttribute(TableCreationStrategy strategy)
    {
        Strategy = strategy;
    }

    public TableCreationStrategy Strategy { get; }
}

public enum TableCreationStrategy
{
    CREATE,
    CREATE_IF_NOT_EXISTS,
    CREATE_OR_REPLACE
}