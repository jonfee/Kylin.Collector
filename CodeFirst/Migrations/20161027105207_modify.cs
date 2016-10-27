using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodeFirst.Migrations
{
    public partial class modify : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "ProductSku",
                type: "datetime",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "ProductSku",
                type: "datetime",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "Product",
                type: "datetime",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Product",
                type: "datetime",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "Category",
                type: "datetime",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Category",
                type: "datetime",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "ProductSku",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "ProductSku",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "Product",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Product",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateTime",
                table: "Category",
                nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateTime",
                table: "Category",
                nullable: false);
        }
    }
}
