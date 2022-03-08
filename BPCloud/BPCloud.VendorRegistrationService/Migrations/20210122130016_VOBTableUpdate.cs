using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud.VendorRegistrationService.Migrations
{
    public partial class VOBTableUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeofIndustry",
                table: "bp_vob",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeofIndustry",
                table: "bp_vob");
        }
    }
}
