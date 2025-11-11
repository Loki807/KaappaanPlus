using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaappanPlus.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAlertResponderWithRestrictDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertResponders_Alerts_AlertId",
                table: "AlertResponders");

            migrationBuilder.AddColumn<string>(
                name: "AssignmentReason",
                table: "AlertResponders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ResponderId",
                table: "AlertResponders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AlertResponders_ResponderId",
                table: "AlertResponders",
                column: "ResponderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertResponders_Alerts_AlertId",
                table: "AlertResponders",
                column: "AlertId",
                principalTable: "Alerts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AlertResponders_AppUsers_ResponderId",
                table: "AlertResponders",
                column: "ResponderId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlertResponders_Alerts_AlertId",
                table: "AlertResponders");

            migrationBuilder.DropForeignKey(
                name: "FK_AlertResponders_AppUsers_ResponderId",
                table: "AlertResponders");

            migrationBuilder.DropIndex(
                name: "IX_AlertResponders_ResponderId",
                table: "AlertResponders");

            migrationBuilder.DropColumn(
                name: "AssignmentReason",
                table: "AlertResponders");

            migrationBuilder.DropColumn(
                name: "ResponderId",
                table: "AlertResponders");

            migrationBuilder.AddForeignKey(
                name: "FK_AlertResponders_Alerts_AlertId",
                table: "AlertResponders",
                column: "AlertId",
                principalTable: "Alerts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
