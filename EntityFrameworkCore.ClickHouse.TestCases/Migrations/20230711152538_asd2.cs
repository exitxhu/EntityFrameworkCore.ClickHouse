using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkCore.ClickHouse.TestCases.Migrations
{
    /// <inheritdoc />
    public partial class asd2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "Int32", nullable: false),
                    Null = table.Column<int>(type: "Int32", nullable: true),
                    MyBool = table.Column<byte>(type: "UInt8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dets");
        }
    }
}
