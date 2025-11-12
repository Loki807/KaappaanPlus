using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KaappanPlus.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class again : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailOtp",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "AppUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "OtpExpiryTime",
                table: "AppUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailOtp",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "OtpExpiryTime",
                table: "AppUsers");
        }
    }
}
