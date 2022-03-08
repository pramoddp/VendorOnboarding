using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthenticationService.Migrations
{
    public partial class UserTableAttemptsAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrevoiusPassword5",
                table: "Users",
                newName: "Pass5");

            migrationBuilder.RenameColumn(
                name: "PrevoiusPassword4",
                table: "Users",
                newName: "Pass4");

            migrationBuilder.RenameColumn(
                name: "PrevoiusPassword3",
                table: "Users",
                newName: "Pass3");

            migrationBuilder.RenameColumn(
                name: "PrevoiusPassword2",
                table: "Users",
                newName: "Pass2");

            migrationBuilder.RenameColumn(
                name: "PrevoiusPassword1",
                table: "Users",
                newName: "Pass1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "Users",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "IsLockDuration",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsLockDuration",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Pass5",
                table: "Users",
                newName: "PrevoiusPassword5");

            migrationBuilder.RenameColumn(
                name: "Pass4",
                table: "Users",
                newName: "PrevoiusPassword4");

            migrationBuilder.RenameColumn(
                name: "Pass3",
                table: "Users",
                newName: "PrevoiusPassword3");

            migrationBuilder.RenameColumn(
                name: "Pass2",
                table: "Users",
                newName: "PrevoiusPassword2");

            migrationBuilder.RenameColumn(
                name: "Pass1",
                table: "Users",
                newName: "PrevoiusPassword1");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiryDate",
                table: "Users",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
