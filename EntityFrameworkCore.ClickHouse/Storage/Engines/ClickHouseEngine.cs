using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;
using static ClickHouse.EntityFrameworkCore.Storage.Engines.ClickHouseEngineTypeConstants;

namespace ClickHouse.EntityFrameworkCore.Storage.Engines;

public abstract class ClickHouseEngine
{
    public abstract string EngineType { get; }

    public abstract void SpecifyEngine(MigrationCommandListBuilder builder, IModel model);
    public abstract string Serialize();
    public ClickHouseEngine Deserialize(string ser, string type)
    {
        return ser.DeserializeToProperType(ClickHouseEngineTypeConstants.GetEngineType(type));
    }
}
public static class ClickHouseEngineDeserilizer
{
    public static ClickHouseEngine DeserializeToProperType(this string ser, ClickHouseEngineType engineType)
    {

        return default(ClickHouseEngine);

    }
}
public class ClickHouseEngineTypeConstants
{
    private static Dictionary<string, ClickHouseEngineType> types = new();
    static ClickHouseEngineTypeConstants()
    {
        types = new Dictionary<string, ClickHouseEngineType>()
        {
            { MergeTreeEngine, new ClickHouseEngineType(MergeTreeEngine) },
            { ReplacingMergeTreeEngine, new ClickHouseEngineType(ReplacingMergeTreeEngine) },
            { StripeLogEngine, new ClickHouseEngineType(StripeLogEngine) },
        };
    }

    public static string MergeTreeEngine = nameof(MergeTreeEngine);
    public static string ReplacingMergeTreeEngine = nameof(ReplacingMergeTreeEngine);
    public static string StripeLogEngine = nameof(StripeLogEngine);

    public static ClickHouseEngineType GetEngineType(string type)
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