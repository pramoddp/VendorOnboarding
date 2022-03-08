using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud.VendorMasterService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cbp_app",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CCode = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cbp_app", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "cbp_bank",
                columns: table => new
                {
                    BankCode = table.Column<string>(nullable: false),
                    BankName = table.Column<string>(nullable: true),
                    BankCity = table.Column<string>(nullable: true),
                    BankCountry = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cbp_bank", x => x.BankCode);
                });

            migrationBuilder.CreateTable(
                name: "cbp_dept",
                columns: table => new
                {
                    Department = table.Column<string>(nullable: false),
                    Language = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cbp_dept", x => new { x.Department, x.Language });
                });

            migrationBuilder.CreateTable(
                name: "cbp_id",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true),
                    Format = table.Column<string>(nullable: true),
                    DocReq = table.Column<string>(nullable: true),
                    ExpDateReq = table.Column<DateTime>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cbp_id", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "cbp_postal",
                columns: table => new
                {
                    PostalCode = table.Column<string>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cbp_postal", x => x.PostalCode);
                });

            migrationBuilder.CreateTable(
                name: "cbp_title",
                columns: table => new
                {
                    Title = table.Column<string>(nullable: false),
                    Language = table.Column<string>(nullable: false),
                    TitleText = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cbp_title", x => new { x.Title, x.Language });
                    table.UniqueConstraint("AK_cbp_title_Language_Title", x => new { x.Language, x.Title });
                });

            migrationBuilder.CreateTable(
                name: "cbp_type",
                columns: table => new
                {
                    Type = table.Column<string>(nullable: false),
                    Language = table.Column<string>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cbp_type", x => new { x.Type, x.Language });
                    table.UniqueConstraint("AK_cbp_type_Language_Type", x => new { x.Language, x.Type });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cbp_app");

            migrationBuilder.DropTable(
                name: "cbp_bank");

            migrationBuilder.DropTable(
                name: "cbp_dept");

            migrationBuilder.DropTable(
                name: "cbp_id");

            migrationBuilder.DropTable(
                name: "cbp_postal");

            migrationBuilder.DropTable(
                name: "cbp_title");

            migrationBuilder.DropTable(
                name: "cbp_type");
        }
    }
}
