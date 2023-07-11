using System;
using System.Diagnostics.CodeAnalysis;
using ClickHouse.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClickHouse.EntityFrameworkCore.Extensions;

public static class ClickHouseMigrationBuilderExtensions
{
    public static bool IsClickHouse([NotNull] this MigrationBuilder migrationBuilder)
        => string.Equals(
            migrationBuilder.ActiveProvider,
            typeof(ClickHouseOptionsExtension).Assembly.GetName().Name,
            StringComparison.Ordinal);
}
