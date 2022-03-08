using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud.VendorMasterService.Migrations
{
    public partial class OBDFieldMasterAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cbp_fieldmaster",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Field = table.Column<string>(nullable: true),
                    FieldName = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    DefaultValue = table.Column<string>(nullable: true),
                    Mandatory = table.Column<bool>(nullable: false),
                    Invisible = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cbp_fieldmaster", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cbp_fieldmaster_Field",
                table: "cbp_fieldmaster",
                column: "Field");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cbp_fieldmaster");
        }
    }
}
