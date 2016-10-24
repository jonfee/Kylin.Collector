using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeFirst.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Icon = table.Column<string>(type: "varchar(300)", nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    Layer = table.Column<string>(type: "varchar(200)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    ParentId = table.Column<long>(nullable: false),
                    Path = table.Column<string>(type: "varchar(500)", nullable: true),
                    Source = table.Column<int>(nullable: false),
                    SourceCategoryId = table.Column<long>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductID = table.Column<long>(nullable: false),
                    BrandID = table.Column<long>(nullable: false),
                    CategoryID = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Intro = table.Column<string>(type: "text", nullable: true),
                    IsDelete = table.Column<bool>(nullable: false),
                    Path = table.Column<string>(type: "varchar(500)", nullable: true),
                    Pics = table.Column<string>(type: "varchar(1000)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    Source = table.Column<int>(nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: false),
                    Weight = table.Column<float>(nullable: false),
                    mainPic = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductID);
                });

            migrationBuilder.CreateTable(
                name: "ProductSku",
                columns: table => new
                {
                    SkuID = table.Column<long>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    ProductID = table.Column<long>(nullable: false),
                    SalePrice = table.Column<decimal>(nullable: false),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSku", x => x.SkuID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ProductSku");
        }
    }
}
