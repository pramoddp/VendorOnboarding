using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Models
{
    public class AuthContext : DbContext
    {
        public AuthContext(DbContextOptions options) : base(options)
        {

        }
        public AuthContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleAppMap>(
            build =>
            {
                build.HasKey(t => new { t.RoleID, t.AppID });
                //build.HasOne(t => t.RoleID).WithOne().HasForeignKey<Role>(qe => qe.RoleID);
                //build.HasOne(t => t.AppID).WithOne().HasForeignKey<App>(qe => qe.AppID);
            });
            modelBuilder.Entity<UserRoleMap>(
            build =>
            {
                build.HasKey(t => new { t.UserID, t.RoleID });
                //build.HasOne(t => t.RoleID).WithOne().HasForeignKey<Role>(qe => qe.RoleID);
                //build.HasOne(t => t.AppID).WithOne().HasForeignKey<App>(qe => qe.AppID);
            });
            modelBuilder.Entity<UserPlantMap>().HasKey(table => new { table.UserID, table.Plant});
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRoleMap> UserRoleMaps { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<RoleAppMap> RoleAppMaps { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistory { get; set; }
        public DbSet<SessionMaster> SessionMasters { get; set; }
        public DbSet<UserPlantMap> UserPlantMaps { get; set; }
        public DbSet<TokenHistory> TokenHistories { get; set; }
    }
}
