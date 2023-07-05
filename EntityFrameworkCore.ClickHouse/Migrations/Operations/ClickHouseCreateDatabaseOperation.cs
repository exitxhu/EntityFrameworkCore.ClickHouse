using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ClickHouse.EntityFrameworkCore.Migrations.Operations;

public class ClickHouseCreateDatabaseOperation : MigrationOperation
{
    public string Name { get; [param: NotNull]set; }
}
