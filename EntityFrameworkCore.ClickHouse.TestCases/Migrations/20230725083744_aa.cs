using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkCore.ClickHouse.TestCases.Migrations
{
    /// <inheritdoc />
    public partial class aa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Order");

            migrationBuilder.CreateTable(
                name: "aa",
                columns: table => new
                {
                    a = table.Column<int>(type: "Int32", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_aa", x => x.a);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "Order",
                columns: table => new
                {
                    OrderId = table.Column<long>(type: "Int64", nullable: false),
                    LinkId = table.Column<int>(type: "Int32", nullable: false),
                    ClickHistoryId = table.Column<long>(type: "Int64", nullable: true),
                    MediaId = table.Column<int>(type: "Int32", nullable: true),
                    WebStoreId = table.Column<int>(type: "Int32", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => new { x.LinkId, x.OrderId });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "aa");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "Order");
        }
    }
}
