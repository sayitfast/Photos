using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Code.Migrations
{
    public partial class SingleImageCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "SingleImages",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SingleImages",
                maxLength: 20,
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SingleImages",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "SingleImages");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SingleImages",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SingleImages",
                nullable: true);
        }
    }
}
