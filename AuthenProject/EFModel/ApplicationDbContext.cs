using AuthenProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.EFModel
{
    public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //AppUser Configuration
            modelBuilder.Entity<AppUser>().ToTable("AppUsers").HasKey(u => u.Id);
            modelBuilder.Entity<AppUser>().Property(u => u.FirstName).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<AppUser>().Property(u => u.LastName).HasMaxLength(100).IsRequired();
            modelBuilder.Entity<AppUser>().Property(u => u.Dob).IsRequired();
            //AppRole Configuration
            modelBuilder.Entity<AppRole>().ToTable("AppRoles").HasKey(r => r.Id);
            modelBuilder.Entity<AppRole>().Property(r => r.Name).HasMaxLength(100).IsRequired();
            //Anonther class in Indentity...
            modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityUserClaim<Guid>>().HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityUserToken<Guid>>().HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(x => new { x.RoleId,x.UserId});

           
        }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }

    }
}
