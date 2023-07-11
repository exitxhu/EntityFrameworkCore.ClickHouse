using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFrameworkCore.ClickHouse.TestCases.Migrations
{
    /// <inheritdoc />
    public partial class asd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<long>(type: "Int64", nullable: false),
                    MediaId = table.Column<int>(type: "Int32", nullable: false),
                    LinkId = table.Column<int>(type: "Int32", nullable: false),
                    LastStatusUpdateDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    PaymentStatus = table.Column<int>(type: "Int32", nullable: false),
                    MediaName = table.Column<string>(type: "String", nullable: false),
                    LinkName = table.Column<string>(type: "String", nullable: false),
                    RefererUserId = table.Column<int>(type: "Int32", nullable: true),
                    Samad = table.Column<long>(type: "Int64", nullable: true)
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
