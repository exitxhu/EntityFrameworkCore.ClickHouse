using ClickHouse.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text.Json;
using static ClickHouse.EntityFrameworkCore.Storage.Engines.ClickHouseEngineTypeConstants;

namespace ClickHouse.EntityFrameworkCore.Storage.Engines;

public abstract class ClickHouseEngine
{
    public abstract string EngineType { get; }

    public abstract void SpecifyEngine(MigrationCommandListBuilder builder, IModel model);
    public abstract string Serialize();
    public static ClickHouseEngine Deserialize(string ser, string type)
    {
        return ser.DeserializeToProperType(GetEngineType(type));
    }
}
public static class ClickHouseEngineDeserilizer
{
    public static ClickHouseEngine DeserializeToProperType(this string ser, Type engineType)
    {
        var res = JsonSerializer.Deserialize(ser, engineType);

        return (ClickHouseEngine)res;

    }
}
public class ClickHouseEngineTypeConstants
{
    private static Dictionary<string, Type> types = new();
    static Func<string, string> engineNamer = (a) => a + "_" + ClickHouseAnnotationNames.Engine;
    static ClickHouseEngineTypeConstants()
    {
        types = new Dictionary<string, Type>()
        {
            { MergeTreeEngine, typeof(MergeTreeEngine) },
            { ReplacingMergeTreeEngine, typeof(ReplacingMergeTreeEngine) },
            { StripeLogEngine, typeof(StripeLogEngine) },
            { PostgreSQLEngine, typeof(PostgreSQLEngine) },
        };
    }
    public static string MergeTreeEngine = engineNamer(nameof(MergeTreeEngine));
    public static string ReplacingMergeTreeEngine = engineNamer(nameof(ReplacingMergeTreeEngine));
    public static string StripeLogEngine = engineNamer(nameof(StripeLogEngine));
    public static string PostgreSQLEngine = engineNamer(nameof(PostgreSQLEngine));

    public static Type GetEngineType(string type)
    {
        if (types.TryGetValue(type, out var res))
            return res;
        throw new KeyNotFoundException($"there is no click house enginetype corresponding to :{type}");
    }
}
public record ClickHouseEngineType
{
    internal ClickHouseEngineType(string EngineType)
    {
        this.EngineType = EngineType;
    }

    public string EngineType { get; }
}