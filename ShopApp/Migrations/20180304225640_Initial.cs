using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ShopApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notebook",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false),
                    GPU = table.Column<string>(nullable: true),
                    Height = table.Column<int>(nullable: false),
                    ImageData = table.Column<byte[]>(nullable: true),
                    Length = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Processor = table.Column<string>(nullable: true),
                    RAM = table.Column<decimal>(nullable: false),
                    ScreenSizeInch = table.Column<decimal>(nullable: false),
                    Width = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notebook", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notebook");
        }
    }
}
