using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace jaug_server_api_core.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tools",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "JS"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "JS"),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Command",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Syntax = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Example = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ToolId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "JS"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "JS"),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Command", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Command_Tools_ToolId",
                        column: x => x.ToolId,
                        principalTable: "Tools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tools",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 1, "", "dotnet" });

            migrationBuilder.InsertData(
                table: "Tools",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 2, "", "docker" });

            migrationBuilder.InsertData(
                table: "Tools",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { 3, "", "git" });

            migrationBuilder.InsertData(
                table: "Command",
                columns: new[] { "Id", "Description", "Example", "Syntax", "ToolId" },
                values: new object[,]
                {
                    { 1, "Enable secret storage for project", null, "dotnet user-secrets init", 1 },
                    { 2, "Set secret for project", null, "dotnet user-secrets set \"<key>\" \"<value>\"", 1 },
                    { 3, "Generate default dotnet gitignore file for project", null, "dotnet new gitignore", 1 },
                    { 4, "Create an empty Git repository or reinitialize an existing one", null, "git init", 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Command_ToolId",
                table: "Command",
                column: "ToolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Command");

            migrationBuilder.DropTable(
                name: "Tools");
        }
    }
}
