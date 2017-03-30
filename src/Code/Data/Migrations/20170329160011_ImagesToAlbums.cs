using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Code.Migrations
{
    public partial class ImagesToAlbums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Album_AlbumId1",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_AlbumId1",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "AlbumId1",
                table: "Images");

            migrationBuilder.AlterColumn<int>(
                name: "AlbumId",
                table: "Images",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Description",
                table: "Images",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Images",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "AlbumId",
                table: "Images",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AlbumId1",
                table: "Images",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_AlbumId1",
                table: "Images",
                column: "AlbumId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Album_AlbumId1",
                table: "Images",
                column: "AlbumId1",
                principalTable: "Album",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
