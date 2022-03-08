using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud.VendorMasterService.Migrations
{
    public partial class StateAndCountryCodeAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "cbp_location",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateCode",
                table: "cbp_location",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "cbp_location");

            migrationBuilder.DropColumn(
                name: "StateCode",
                table: "cbp_location");
        }
    }
}
