using System;
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

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<long>(type: "Int64", nullable: false),
                    MediaId = table.Column<int>(type: "Int32", nullable: false),
                    LastStatusUpdateDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PaymentStatus = table.Column<int>(type: "Int32", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dets");

            migrationBuilder.DropTable(
                name: "Order");
        }
    }
}
