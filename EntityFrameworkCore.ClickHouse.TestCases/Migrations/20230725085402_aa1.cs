using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkCore.ClickHouse.TestCases.Migrations
{
    /// <inheritdoc />
    public partial class aa1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                schema: "Order",
                table: "Order");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                schema: "Order",
                table: "Order",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                schema: "Order",
                table: "Order");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                schema: "Order",
                table: "Order",
                columns: new[] { "LinkId", "OrderId" });
        }
    }
}
