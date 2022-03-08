using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud.VendorRegistrationService.Migrations
{
    public partial class DepartmentPGAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "bp_vob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PurchaseGroup",
                table: "bp_vob",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Department",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "PurchaseGroup",
                table: "bp_vob");
        }
    }
}
