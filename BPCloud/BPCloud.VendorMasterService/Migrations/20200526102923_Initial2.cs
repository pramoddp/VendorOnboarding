using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud.VendorMasterService.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankBranch",
                table: "cbp_bank",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "cbp_gstin",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Gstin = table.Column<string>(nullable: false),
                    CustomerUserName = table.Column<string>(nullable: true),
                    CustomerPassword = table.Column<string>(nullable: true),
                    GSPClientID = table.Column<string>(nullable: true),
                    GSPClientSecret = table.Column<string>(nullable: true),
                    PublicKeyPath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cbp_gstin", x => x.Gstin);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cbp_gstin");

            migrationBuilder.DropColumn(
                name: "BankBranch",
                table: "cbp_bank");
        }
    }
}
