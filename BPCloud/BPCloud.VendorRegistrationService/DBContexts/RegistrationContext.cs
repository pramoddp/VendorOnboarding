using BPCloud.VendorRegistrationService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BPCloud.VendorRegistrationService.DBContexts
{
    public class RegistrationContext : DbContext
    {
        public RegistrationContext(DbContextOptions<RegistrationContext> options) : base(options) { }
        public RegistrationContext() { }

        public DbSet<BPVendorOnBoarding> BPVendorOnBoardings { get; set; }
        public DbSet<BPIdentity> BPIdentities { get; set; }
        public DbSet<BPBank> BPBanks { get; set; }
      
       // public DbSet<BPIdentity> BPIdentity { get; set; }
       
        public DbSet<BPContact> BPContacts { get; set; }
        public DbSet<BPActivityLog> BPActivityLogs { get; set; }
        public DbSet<BPText> BPTexts { get; set; }
        public DbSet<BPAttachment> BPAttachments { get; set; }
        public DbSet<TokenHistory> TokenHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BPIdentity>().HasKey(table => new { table.TransID, table.Type });
            modelBuilder.Entity<BPBank>().HasKey(table => new { table.TransID, table.AccountNo });
            modelBuilder.Entity<BPContact>().HasKey(table => new { table.TransID, table.Item });
            modelBuilder.Entity<BPActivityLog>().HasKey(table => new { table.TransID, table.LogID });
        }
    }
}
