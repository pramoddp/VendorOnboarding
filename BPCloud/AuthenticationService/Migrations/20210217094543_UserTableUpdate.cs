using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthenticationService.Migrations
{
    public partial class UserTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PrevoiusPassword1",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrevoiusPassword2",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrevoiusPassword3",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrevoiusPassword4",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrevoiusPassword5",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrevoiusPassword1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PrevoiusPassword2",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PrevoiusPassword3",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PrevoiusPassword4",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PrevoiusPassword5",
                table: "Users");
        }
    }
}
