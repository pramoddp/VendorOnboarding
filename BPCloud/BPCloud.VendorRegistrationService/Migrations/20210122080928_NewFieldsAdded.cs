using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud.VendorRegistrationService.Migrations
{
    public partial class NewFieldsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PurchaseGroup",
                table: "bp_vob",
                newName: "PurchaseOrg");

            migrationBuilder.AddColumn<string>(
                name: "AccountGroup",
                table: "bp_vob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyCode",
                table: "bp_vob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmamiContactPerson",
                table: "bp_vob",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmamiContactPersonMail",
                table: "bp_vob",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountGroup",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "CompanyCode",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "EmamiContactPerson",
                table: "bp_vob");

            migrationBuilder.DropColumn(
                name: "EmamiContactPersonMail",
                table: "bp_vob");

            migrationBuilder.RenameColumn(
                name: "PurchaseOrg",
                table: "bp_vob",
                newName: "PurchaseGroup");
        }
    }
}
