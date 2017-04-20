using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Code.Migrations
{
    public partial class SingleImagePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "SingleImages",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "SingleImages",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "SingleImages");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "SingleImages",
                nullable: true);
        }
    }
}
