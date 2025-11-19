using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaappanPlus.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationLogs_AppUsers_UserId",
                table: "LocationLogs");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "LocationLogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "CitizenId",
                table: "LocationLogs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "AlertResponders",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "AlertResponders",
                type: "float",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationLogs_CitizenId",
                table: "LocationLogs",
                column: "CitizenId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationLogs_AppUsers_UserId",
                table: "LocationLogs",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationLogs_Citizens_CitizenId",
                table: "LocationLogs",
                column: "CitizenId",
                principalTable: "Citizens",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationLogs_AppUsers_UserId",
                table: "LocationLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationLogs_Citizens_CitizenId",
                table: "LocationLogs");

            migrationBuilder.DropIndex(
                name: "IX_LocationLogs_CitizenId",
                table: "LocationLogs");

            migrationBuilder.DropColumn(
                name: "CitizenId",
                table: "LocationLogs");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "AlertResponders");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "AlertResponders");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "LocationLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationLogs_AppUsers_UserId",
                table: "LocationLogs",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
