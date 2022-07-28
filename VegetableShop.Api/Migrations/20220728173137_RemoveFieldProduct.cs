using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VegetableShop.Api.Migrations
{
    public partial class RemoveFieldProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "Products",
                type: "datetime2",
                nullable: true);
        }
    }
}
