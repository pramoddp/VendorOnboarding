using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BPCloud.VendorRegistrationService.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bp_doc",
                columns: table => new
                {
                    AttachmentID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ProjectName = table.Column<string>(nullable: true),
                    AppID = table.Column<int>(nullable: false),
                    AppNumber = table.Column<string>(nullable: true),
                    IsHeaderExist = table.Column<bool>(nullable: false),
                    HeaderNumber = table.Column<string>(nullable: true),
                    AttachmentName = table.Column<string>(nullable: true),
                    ContentType = table.Column<string>(nullable: true),
                    ContentLength = table.Column<long>(nullable: false),
                    AttachmentFile = table.Column<byte[]>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bp_doc", x => x.AttachmentID);
                });

            migrationBuilder.CreateTable(
                name: "bp_text",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    TextID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bp_text", x => x.TextID);
                });

            migrationBuilder.CreateTable(
                name: "bp_vob",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    TransID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true),
                    LegalName = table.Column<string>(nullable: true),
                    AddressLine1 = table.Column<string>(nullable: true),
                    AddressLine2 = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    PinCode = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Plant = table.Column<string>(nullable: true),
                    GSTNumber = table.Column<string>(nullable: true),
                    GSTStatus = table.Column<string>(nullable: true),
                    PANNumber = table.Column<string>(nullable: true),
                    Phone1 = table.Column<string>(nullable: true),
                    Phone2 = table.Column<string>(nullable: true),
                    Email1 = table.Column<string>(nullable: true),
                    Email2 = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    MSME = table.Column<bool>(nullable: false),
                    MSME_TYPE = table.Column<string>(nullable: true),
                    MSME_Att_ID = table.Column<string>(nullable: true),
                    Reduced_TDS = table.Column<bool>(nullable: false),
                    TDS_RATE = table.Column<string>(nullable: true),
                    TDS_Att_ID = table.Column<string>(nullable: true),
                    RP = table.Column<bool>(nullable: false),
                    RP_Name = table.Column<string>(nullable: true),
                    RP_Type = table.Column<string>(nullable: true),
                    RP_Att_ID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bp_vob", x => x.TransID);
                });

            migrationBuilder.CreateTable(
                name: "TokenHistories",
                columns: table => new
                {
                    TokenHistoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransID = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    OTP = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    ExpireOn = table.Column<DateTime>(nullable: false),
                    UsedOn = table.Column<DateTime>(nullable: true),
                    IsUsed = table.Column<bool>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenHistories", x => x.TokenHistoryID);
                });

            migrationBuilder.CreateTable(
                name: "bp_act_log",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    LogID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransID = table.Column<int>(nullable: false),
                    Activity = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Time = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bp_act_log", x => new { x.TransID, x.LogID });
                    table.UniqueConstraint("AK_bp_act_log_LogID", x => x.LogID);
                    table.ForeignKey(
                        name: "FK_bp_act_log_bp_vob_TransID",
                        column: x => x.TransID,
                        principalTable: "bp_vob",
                        principalColumn: "TransID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bp_bank",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    TransID = table.Column<int>(nullable: false),
                    AccountNo = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IFSC = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    Branch = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    DocID = table.Column<string>(nullable: true),
                    AttachmentName = table.Column<string>(nullable: true),
                    IsValid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bp_bank", x => new { x.TransID, x.AccountNo });
                    table.UniqueConstraint("AK_bp_bank_AccountNo_TransID", x => new { x.AccountNo, x.TransID });
                    table.ForeignKey(
                        name: "FK_bp_bank_bp_vob_TransID",
                        column: x => x.TransID,
                        principalTable: "bp_vob",
                        principalColumn: "TransID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bp_contact",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    TransID = table.Column<int>(nullable: false),
                    Item = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bp_contact", x => new { x.TransID, x.Item });
                    table.UniqueConstraint("AK_bp_contact_Item_TransID", x => new { x.Item, x.TransID });
                    table.ForeignKey(
                        name: "FK_bp_contact_bp_vob_TransID",
                        column: x => x.TransID,
                        principalTable: "bp_vob",
                        principalColumn: "TransID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bp_id",
                columns: table => new
                {
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    TransID = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    Option = table.Column<string>(nullable: true),
                    IDNumber = table.Column<string>(nullable: true),
                    ValidUntil = table.Column<DateTime>(nullable: true),
                    DocID = table.Column<string>(nullable: true),
                    AttachmentName = table.Column<string>(nullable: true),
                    AttachmentContents = table.Column<string>(nullable: true),
                    IsValid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bp_id", x => new { x.TransID, x.Type });
                    table.ForeignKey(
                        name: "FK_bp_id_bp_vob_TransID",
                        column: x => x.TransID,
                        principalTable: "bp_vob",
                        principalColumn: "TransID",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bp_act_log");

            migrationBuilder.DropTable(
                name: "bp_bank");

            migrationBuilder.DropTable(
                name: "bp_contact");

            migrationBuilder.DropTable(
                name: "bp_doc");

            migrationBuilder.DropTable(
                name: "bp_id");

            migrationBuilder.DropTable(
                name: "bp_text");

            migrationBuilder.DropTable(
                name: "TokenHistories");

            migrationBuilder.DropTable(
                name: "bp_vob");
        }
    }
}
