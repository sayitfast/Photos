using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Code.Migrations
{
    public partial class ModelChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AlbumId",
                table: "Images",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_AlbumId",
                table: "Images",
                column: "AlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Album_AlbumId",
                table: "Images",
                column: "AlbumId",
                principalTable: "Album",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Album_AlbumId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_AlbumId",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "AlbumId",
                table: "Images",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
