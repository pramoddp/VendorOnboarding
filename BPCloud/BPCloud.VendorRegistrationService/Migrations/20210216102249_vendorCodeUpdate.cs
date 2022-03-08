using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud.VendorRegistrationService.Migrations
{
    public partial class vendorCodeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VendorCode",
                table: "bp_vob",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorCode",
                table: "bp_vob");
        }
    }
}
