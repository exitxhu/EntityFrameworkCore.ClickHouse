using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkCore.ClickHouse.TestCases.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "Int32", nullable: false),
                    Mobile = table.Column<string>(type: "String", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "String", nullable: false),
                    Agreement = table.Column<byte>(type: "UInt8", nullable: false),
                    KycStatus = table.Column<int[]>(type: "Array(Int32)", nullable: false),
                    Status = table.Column<int>(type: "Int32", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
