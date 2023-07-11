using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkCore.ClickHouse.TestCases.Migrations
{
    /// <inheritdoc />
    public partial class asd4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MediaId",
                table: "Order",
                newName: "ShortId");

            migrationBuilder.AddColumn<int>(
                name: "MediaaaaId",
                table: "Order",
                type: "Int32",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MediaaaaId",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "ShortId",
                table: "Order",
                newName: "MediaId");
        }
    }
}
