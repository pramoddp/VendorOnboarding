using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud.VendorMasterService.Migrations
{
    public partial class IndentityTableUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpDateReq",
                table: "cbp_id");

            migrationBuilder.RenameColumn(
                name: "Format",
                table: "cbp_id",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "DocReq",
                table: "cbp_id",
                newName: "RegexFormat");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "cbp_id",
                newName: "FileFormat");

            migrationBuilder.AddColumn<bool>(
                name: "Mandatory",
                table: "cbp_id",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "MaxSizeInKB",
                table: "cbp_id",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mandatory",
                table: "cbp_id");

            migrationBuilder.DropColumn(
                name: "MaxSizeInKB",
                table: "cbp_id");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "cbp_id",
                newName: "Format");

            migrationBuilder.RenameColumn(
                name: "RegexFormat",
                table: "cbp_id",
                newName: "DocReq");

            migrationBuilder.RenameColumn(
                name: "FileFormat",
                table: "cbp_id",
                newName: "Country");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpDateReq",
                table: "cbp_id",
                nullable: true);
        }
    }
}
