using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Isjr.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MultimediaTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultimediaTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Hash = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Salt = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MultimediaItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(nullable: true),
                    TypeId = table.Column<int>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultimediaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MultimediaItems_MultimediaTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "MultimediaTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JojoReferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Header = table.Column<string>(maxLength: 50, nullable: false),
                    OriginalId = table.Column<int>(nullable: true),
                    ReferenceId = table.Column<int>(nullable: true),
                    Text = table.Column<string>(maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JojoReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JojoReferences_MultimediaItems_OriginalId",
                        column: x => x.OriginalId,
                        principalTable: "MultimediaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JojoReferences_MultimediaItems_ReferenceId",
                        column: x => x.ReferenceId,
                        principalTable: "MultimediaItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JojoReferences_OriginalId",
                table: "JojoReferences",
                column: "OriginalId");

            migrationBuilder.CreateIndex(
                name: "IX_JojoReferences_ReferenceId",
                table: "JojoReferences",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_MultimediaItems_TypeId",
                table: "MultimediaItems",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JojoReferences");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "MultimediaItems");

            migrationBuilder.DropTable(
                name: "MultimediaTypes");
        }
    }
}
