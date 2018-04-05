using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Piranha.Migrations
{
    [NoCoverage]
    public partial class AddBlockTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Piranha_BlockTypes",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 64, nullable: false),
                    Body = table.Column<string>(nullable: true),
                    CLRType = table.Column<string>(maxLength: 256, nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    LastModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Piranha_BlockTypes", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Piranha_BlockTypes");
        }
    }
}
