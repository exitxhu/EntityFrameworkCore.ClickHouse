using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkCore.ClickHouse.TestCases.Migrations
{
    /// <inheritdoc />
    public partial class asd1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "Int32", nullable: false),
                    Null = table.Column<int>(type: "Int32", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dets", x => x.Id);
                });
        }
    }
}
