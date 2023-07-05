using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ClickHouse.EntityFrameworkCore.Migrations.Operations;

public class ClickHouseDropDatabaseOperation : MigrationOperation
{
    public string Name { get; [param: NotNull]set; }
}