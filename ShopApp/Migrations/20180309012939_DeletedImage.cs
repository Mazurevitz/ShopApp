using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShopApp.Migrations
{
    public partial class DeletedImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Notebook");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Notebook");

            migrationBuilder.DropColumn(
                name: "Length",
                table: "Notebook");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Notebook");

            migrationBuilder.AlterColumn<int>(
                name: "ID",
                table: "Notebook",
                nullable: false,
                oldClrType: typeof(string))
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ID",
                table: "Notebook",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Notebook",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Notebook",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Length",
                table: "Notebook",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Notebook",
                nullable: false,
                defaultValue: 0);
        }
    }
}
