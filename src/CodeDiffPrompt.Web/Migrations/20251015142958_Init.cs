using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeDiffPrompt.Web.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CodeSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeSnapshots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PromptRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Language = table.Column<string>(type: "TEXT", nullable: false),
                    PromptText = table.Column<string>(type: "TEXT", nullable: false),
                    DiffText = table.Column<string>(type: "TEXT", nullable: false),
                    LlmResponse = table.Column<string>(type: "TEXT", nullable: true),
                    FileName = table.Column<string>(type: "TEXT", nullable: true),
                    BeforeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    AfterId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromptRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PromptRecords_CodeSnapshots_AfterId",
                        column: x => x.AfterId,
                        principalTable: "CodeSnapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PromptRecords_CodeSnapshots_BeforeId",
                        column: x => x.BeforeId,
                        principalTable: "CodeSnapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromptRecords_AfterId",
                table: "PromptRecords",
                column: "AfterId");

            migrationBuilder.CreateIndex(
                name: "IX_PromptRecords_BeforeId",
                table: "PromptRecords",
                column: "BeforeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromptRecords");

            migrationBuilder.DropTable(
                name: "CodeSnapshots");
        }
    }
}
