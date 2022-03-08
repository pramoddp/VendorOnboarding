﻿// <auto-generated />
using System;
using BPCloud.VendorRegistrationService.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BPCloud.VendorRegistrationService.Migrations
{
    [DbContext(typeof(RegistrationContext))]
    [Migration("20210216102249_vendorCodeUpdate")]
    partial class vendorCodeUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BPCloud.VendorRegistrationService.Models.BPActivityLog", b =>
                {
                    b.Property<int>("TransID");

                    b.Property<int>("LogID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Activity");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<DateTime?>("Date");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Text");

                    b.Property<string>("Time");

                    b.HasKey("TransID", "LogID");

                    b.HasAlternateKey("LogID");

                    b.ToTable("bp_act_log");
                });

            modelBuilder.Entity("BPCloud.VendorRegistrationService.Models.BPAttachment", b =>
                {
                    b.Property<int>("AttachmentID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AppID");

                    b.Property<string>("AppNumber");

                    b.Property<byte[]>("AttachmentFile");

                    b.Property<string>("AttachmentName");

                    b.Property<long>("ContentLength");

                    b.Property<string>("ContentType");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("HeaderNumber");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsHeaderExist");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("ProjectName");

                    b.HasKey("AttachmentID");

                    b.ToTable("bp_doc");
                });

            modelBuilder.Entity("BPCloud.VendorRegistrationService.Models.BPBank", b =>
                {
                    b.Property<int>("TransID");

                    b.Property<string>("AccountNo");

                    b.Property<string>("AttachmentName");

                    b.Property<string>("BankName");

                    b.Property<string>("Branch");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("DocID");

                    b.Property<string>("IFSC");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsValid");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name");

                    b.HasKey("TransID", "AccountNo");

                    b.HasAlternateKey("AccountNo", "TransID");

                    b.ToTable("bp_bank");
                });

            modelBuilder.Entity("BPCloud.VendorRegistrationService.Models.BPContact", b =>
                {
                    b.Property<int>("TransID");

                    b.Property<string>("Item");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Department");

                    b.Property<string>("Email");

                    b.Property<bool>("IsActive");

                    b.Property<string>("Mobile");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<string>("Title");

                    b.HasKey("TransID", "Item");

                    b.HasAlternateKey("Item", "TransID");

                    b.ToTable("bp_contact");
                });

            modelBuilder.Entity("BPCloud.VendorRegistrationService.Models.BPIdentity", b =>
                {
                    b.Property<int>("TransID");

                    b.Property<string>("Type");

                    b.Property<string>("AttachmentContents");

                    b.Property<string>("AttachmentName");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("DocID");

                    b.Property<string>("IDNumber");

                    b.Property<bool>("IsActive");

                    b.Property<bool>("IsValid");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Option");

                    b.Property<DateTime?>("ValidUntil");

                    b.HasKey("TransID", "Type");

                    b.ToTable("bp_id");
                });

            modelBuilder.Entity("BPCloud.VendorRegistrationService.Models.BPText", b =>
                {
                    b.Property<int>("TextID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<bool>("IsActive");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Text");

                    b.HasKey("TextID");

                    b.ToTable("bp_text");
                });

            modelBuilder.Entity("BPCloud.VendorRegistrationService.Models.BPVendorOnBoarding", b =>
                {
                    b.Property<int>("TransID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccountGroup");

                    b.Property<string>("AddressLine1");

                    b.Property<string>("AddressLine2");

                    b.Property<string>("City");

                    b.Property<string>("CompanyCode");

                    b.Property<string>("Country");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("Department");

                    b.Property<string>("Email1");

                    b.Property<string>("Email2");

                    b.Property<string>("EmamiContactPerson");

                    b.Property<string>("EmamiContactPersonMail");

                    b.Property<string>("GSTNumber");

                    b.Property<string>("GSTStatus");

                    b.Property<bool>("IsActive");

                    b.Property<string>("LegalName");

                    b.Property<bool>("MSME");

                    b.Property<string>("MSME_Att_ID");

                    b.Property<string>("MSME_TYPE");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Name");

                    b.Property<string>("PANNumber");

                    b.Property<string>("Phone1");

                    b.Property<string>("Phone2");

                    b.Property<string>("PinCode");

                    b.Property<string>("Plant");

                    b.Property<string>("PurchaseOrg");

                    b.Property<bool>("RP");

                    b.Property<string>("RP_Att_ID");

                    b.Property<string>("RP_Name");

                    b.Property<string>("RP_Type");

                    b.Property<bool>("Reduced_TDS");

                    b.Property<string>("Remarks");

                    b.Property<string>("Role");

                    b.Property<string>("State");

                    b.Property<string>("Status");

                    b.Property<string>("TDS_Att_ID");

                    b.Property<string>("TDS_RATE");

                    b.Property<string>("Type");

                    b.Property<string>("TypeofIndustry");

                    b.Property<string>("VendorCode");

                    b.HasKey("TransID");

                    b.ToTable("bp_vob");
                });

            modelBuilder.Entity("BPCloud.VendorRegistrationService.Models.TokenHistory", b =>
                {
                    b.Property<int>("TokenHistoryID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Comment");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<string>("EmailAddress");

                    b.Property<DateTime>("ExpireOn");

                    b.Property<bool>("IsUsed");

                    b.Property<string>("OTP");

                    b.Property<string>("Token");

                    b.Property<int>("TransID");

                    b.Property<DateTime?>("UsedOn");

                    b.Property<string>("UserName");

                    b.HasKey("TokenHistoryID");

                    b.ToTable("TokenHistories");
                });

            modelBuilder.Entity("BPCloud.VendorRegistrationService.Models.BPActivityLog", b =>
                {
                    b.HasOne("BPCloud.VendorRegistrationService.Models.BPVendorOnBoarding", "BPVendorOnBoarding")
                        .WithMany()
                        .HasForeignKey("TransID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BPCloud.VendorRegistrationService.Models.BPBank", b =>
                {
                    b.HasOne("BPCloud.VendorRegistrationService.Models.BPVendorOnBoarding", "BPVendorOnBoarding")
                        .WithMany()
                        .HasForeignKey("TransID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BPCloud.VendorRegistrationService.Models.BPContact", b =>
                {
                    b.HasOne("BPCloud.VendorRegistrationService.Models.BPVendorOnBoarding", "BPVendorOnBoarding")
                        .WithMany()
                        .HasForeignKey("TransID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("BPCloud.VendorRegistrationService.Models.BPIdentity", b =>
                {
                    b.HasOne("BPCloud.VendorRegistrationService.Models.BPVendorOnBoarding", "BPVendorOnBoarding")
                        .WithMany()
                        .HasForeignKey("TransID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
