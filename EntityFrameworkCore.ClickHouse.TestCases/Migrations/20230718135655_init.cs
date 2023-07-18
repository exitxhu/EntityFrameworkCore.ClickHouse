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
                name: "Links",
                columns: table => new
                {
                    LinkId = table.Column<int>(type: "Int32", nullable: false),
                    Name = table.Column<string>(type: "String", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.LinkId);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    MediaId = table.Column<int>(type: "Int32", nullable: false),
                    Name = table.Column<string>(type: "String", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.MediaId);
                });

            migrationBuilder.CreateTable(
                name: "WebStore",
                columns: table => new
                {
                    WebStoreId = table.Column<int>(type: "Int32", nullable: false),
                    AlternativeUrl = table.Column<string>(type: "String", nullable: false),
                    RecheckHeaders = table.Column<string[]>(type: "Array(String)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    RecheckFromDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    About = table.Column<string>(type: "String", nullable: false),
                    TradeName = table.Column<string>(type: "String", nullable: false),
                    DependingWebStoreId = table.Column<int>(type: "Int32", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebStore", x => x.WebStoreId);
                    table.ForeignKey(
                        name: "FK_WebStore_WebStore_DependingWebStoreId",
                        column: x => x.DependingWebStoreId,
                        principalTable: "WebStore",
                        principalColumn: "WebStoreId");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<long>(type: "Int64", nullable: false),
                    LinkId = table.Column<int>(type: "Int32", nullable: true),
                    MediaId = table.Column<int>(type: "Int32", nullable: true),
                    WebStoreId = table.Column<int>(type: "Int32", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_Links_LinkId",
                        column: x => x.LinkId,
                        principalTable: "Links",
                        principalColumn: "LinkId");
                    table.ForeignKey(
                        name: "FK_Order_Media_MediaId",
                        column: x => x.MediaId,
                        principalTable: "Media",
                        principalColumn: "MediaId");
                    table.ForeignKey(
                        name: "FK_Order_WebStore_WebStoreId",
                        column: x => x.WebStoreId,
                        principalTable: "WebStore",
                        principalColumn: "WebStoreId");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "WebStore");
        }
    }
}
