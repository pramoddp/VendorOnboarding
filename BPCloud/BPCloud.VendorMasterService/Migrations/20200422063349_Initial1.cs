using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud.VendorMasterService.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cbp_location",
                columns: table => new
                {
                    Pincode = table.Column<string>(maxLength: 20, nullable: false),
                    Location = table.Column<string>(maxLength: 100, nullable: true),
                    Taluk = table.Column<string>(maxLength: 100, nullable: true),
                    District = table.Column<string>(maxLength: 100, nullable: true),
                    State = table.Column<string>(maxLength: 100, nullable: true),
                    Country = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cbp_location", x => x.Pincode);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cbp_location");
        }
    }
}
