using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeFirst.Migrations
{
    public partial class modifyProduct_SKU : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SourceSkuId",
                table: "ProductSku",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<float>(
                name: "Weight",
                table: "ProductSku",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<long>(
                name: "SourceProductID",
                table: "Product",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SourceSkuId",
                table: "ProductSku");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "ProductSku");

            migrationBuilder.DropColumn(
                name: "SourceProductID",
                table: "Product");
        }
    }
}
