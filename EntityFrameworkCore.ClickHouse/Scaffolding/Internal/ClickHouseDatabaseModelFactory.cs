using ClickHouse.Client.ADO;
using ClickHouse.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using ClickHouse.EntityFrameworkCore.Infrastructure;

namespace ClickHouse.EntityFrameworkCore.Scaffolding.Internal;

public class ClickHouseDatabaseModelFactory : DatabaseModelFactory
{
    public override DatabaseModel Create(string connectionString, DatabaseModelFactoryOptions options)
    {
        var connection = new ClickHouseConnection(connectionString);
        return Create(connection, options);
    }

    public override DatabaseModel Create(DbConnection connection, DatabaseModelFactoryOptions options)
    {
        var sb = new ClickHouseConnectionStringBuilder(connection.ConnectionString);
        var result = new DatabaseModel { DatabaseName = sb.Database };
        var tables = LoadTables(connection, result);
        tables.ForEach(e => result.Tables.Add(e));
        return result;
    }

    private List<DatabaseTable> LoadTables(DbConnection connection, DatabaseModel database)
    {
        var result = new List<DatabaseTable>();
        var primaryKeys = new Dictionary<string, string[]>();
        var query = $"SELECT * FROM system.tables WHERE database='{database.DatabaseName}';";

        connection.Open();

        using (var command = connection.CreateCommand(query))
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var table = new DatabaseTable
                {
                    Database = database,
                    Name = reader.GetString("name")
                };

                var primaryKey = reader.GetString("primary_key");

                if (!string.IsNullOrEmpty(primaryKey))
                {
                    var primaryKeyColumns = Array.ConvertAll(primaryKey.Split(','), e => e.Trim());
                    primaryKeys[table.Name] = primaryKeyColumns;
                }
                
                result.Add(table);
            }

            connection.Close();
            LoadColumns(connection, result, database, primaryKeys);
            // TODO LoadConstraints(...);
            return result;
        }
    }

    private void LoadColumns(
        DbConnection connection,
        List<DatabaseTable> tables,
        DatabaseModel database,
        Dictionary<string, string[]> primaryKeys)
    {
        if (tables.Count == 0)
        {
            return;
        }

        connection.Open();
        var tablesQ = string.Join(", ", tables.Select(e => $"'{e.Name}'"));
        var query = $"SELECT * FROM system.columns WHERE database='{database.DatabaseName}' AND name IN ({tablesQ});";

        using (var command = connection.CreateCommand(query))
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var tableName = reader.GetString("name");
                var table = tables.Single(e => e.Name == tableName);
                var column = new DatabaseColumn
                {
                    Comment = reader.GetString("comment"),
                    StoreType = reader.GetString("type"),
                    IsNullable = reader.GetString("type").StartsWith("Nullable"),
                    DefaultValueSql = reader.GetString("default_expression"),
                    Name = reader.GetString("name"),
                    Table = table
                };

                table.Columns.Add(column);
            }

            connection.Close();

            foreach (var primaryKey in primaryKeys)
            {
                var table = tables.Single(e => e.Name == primaryKey.Key);
                table.PrimaryKey = new DatabasePrimaryKey();

                foreach (var columnName in primaryKey.Value)
                {
                    var column = table.Columns.Single(e => e.Name == columnName);
                    table.PrimaryKey.Columns.Add(column);
                }
            }
        }
    }
}


//public class ClickHouseCodeGenerator : ProviderCodeGenerator
//{
   
//    /// <summary>
//    ///     Initializes a new instance of the <see cref="SqlServerCodeGenerator" /> class.
//    /// </summary>
//    /// <param name="dependencies">The dependencies.</param>
//    public ClickHouseCodeGenerator(ProviderCodeGeneratorDependencies dependencies)
//        : base(dependencies)
//    {
//    }
//    private static readonly MethodInfo UseSqlServerMethodInfo
//       = typeof(ClickHouseDbContextOptionsBuilderExtensions).GetRuntimeMethod(
//           nameof(ClickHouseDbContextOptionsBuilderExtensions.UseClickHouse),
//           new[] { typeof(DbContextOptionsBuilder), typeof(string), typeof(Action<ClickHouseDbContextOptionsBuilder>) })!;

//    /// <summary>
//    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
//    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
//    ///     any release. You should only use it directly in your code with extreme caution and knowing that
//    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
//    /// </summary>
//    public override MethodCallCodeFragment GenerateUseProvider(
//        string connectionString,
//        MethodCallCodeFragment? providerOptions)
//        => new(
//            UseSqlServerMethodInfo,
//            providerOptions == null
//                ? new object[] { connectionString }
//                : new object[] { connectionString, new NestedClosureCodeFragment("x", providerOptions) });
//}
