using BPCloud.VendorMasterService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorMasterService.DBContexts
{
    public class MasterContext : DbContext
    {
        public MasterContext(DbContextOptions<MasterContext> options) : base(options) { }
        public MasterContext() { }

        public DbSet<CBPType> CBPTypes { get; set; }
        public DbSet<CBPPostal> CBPPostals { get; set; }
        public DbSet<CBPIdentity> CBPIdentities { get; set; }
        public DbSet<CBPBank> CBPBanks { get; set; }
        public DbSet<CBPTitle> CBPTitles { get; set; }
        public DbSet<CBPDepartment> CBPDepartments { get; set; }
        public DbSet<CBPApp> CBPApps { get; set; }
        public DbSet<CBPLocation> CBPLocations { get; set; }
        public DbSet<CBPGstin> CBPGstins { get; set; }
        public DbSet<CBPFieldMaster> CBPFieldMasters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CBPType>().HasKey(table => new {
                table.Type,
                table.Language
            });
            modelBuilder.Entity<CBPTitle>().HasKey(table => new {
                table.Title,
                table.Language
            });
            modelBuilder.Entity<CBPDepartment>().HasKey(table => new {
                table.Department,
                table.Language
            });
            modelBuilder.Entity<CBPFieldMaster>().HasKey(table => new { table.ID });
            modelBuilder.Entity<CBPFieldMaster>().HasIndex(table => new { table.Field });
        }
    }
}
