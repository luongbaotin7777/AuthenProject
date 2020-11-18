using AuthenProject.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenProject.SeedingData
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<IdentityRoleClaim<Guid>>().HasData(
            //                //Set permission for USER
            //                new IdentityRoleClaim<string> { Id = 1, RoleId = "63931105-2B4B-4EB3-2AD4-08D8878A18F1", ClaimType = "permission", ClaimValue = "Permission.Products.View" },
            //                new IdentityRoleClaim<string> { Id = 2, RoleId = "63931105-2B4B-4EB3-2AD4-08D8878A18F1", ClaimType = "permission", ClaimValue = "Permission.Categories.View" },
            //                new IdentityRoleClaim<string> { Id = 3, RoleId = "63931105-2B4B-4EB3-2AD4-08D8878A18F1", ClaimType = "permission", ClaimValue = "Permission.Dashboards.View" },
            //                //Set permission for ADMIN
            //                new IdentityRoleClaim<string> { Id = 4, RoleId = "FFD36419-3983-4DCE-12AE-08D8877FE392", ClaimType = "permission", ClaimValue = "Permission.Categories.View" },
            //                new IdentityRoleClaim<string> { Id = 5, RoleId = "FFD36419-3983-4DCE-12AE-08D8877FE392", ClaimType = "permission", ClaimValue = "Permission.Categories.Create" },
            //                new IdentityRoleClaim<string> { Id = 6, RoleId = "FFD36419-3983-4DCE-12AE-08D8877FE392", ClaimType = "permission", ClaimValue = "Permission.Categories.Edit" },
            //                new IdentityRoleClaim<string> { Id = 7, RoleId = "FFD36419-3983-4DCE-12AE-08D8877FE392", ClaimType = "permission", ClaimValue = "Permission.Categories.Delete" },

            //                new IdentityRoleClaim<string> { Id = 8, RoleId = "FFD36419-3983-4DCE-12AE-08D8877FE392", ClaimType = "permission", ClaimValue = "Permission.Products.View" },
            //                new IdentityRoleClaim<string> { Id = 9, RoleId = "FFD36419-3983-4DCE-12AE-08D8877FE392", ClaimType = "permission", ClaimValue = "Permission.Products.Create" },
            //                new IdentityRoleClaim<string> { Id = 10, RoleId = "FFD36419-3983-4DCE-12AE-08D8877FE392", ClaimType = "permission", ClaimValue = "Permission.Products.Edit" },
            //                new IdentityRoleClaim<string> { Id = 11, RoleId = "FFD36419-3983-4DCE-12AE-08D8877FE392", ClaimType = "permission", ClaimValue = "Permission.Products.Delete" },

            //                new IdentityRoleClaim<string> { Id = 12, RoleId = "FFD36419-3983-4DCE-12AE-08D8877FE392", ClaimType = "permission", ClaimValue = "Permission.Dashboards.View" },

            //                //Set permission for MOD
            //                new IdentityRoleClaim<string> { Id = 13, RoleId = "8A02C605-8B6C-43DD-2AD5-08D8878A18F1", ClaimType = "permission", ClaimValue = "Permission.Dashboards.View" },

            //                new IdentityRoleClaim<string> { Id = 14, RoleId = "8A02C605-8B6C-43DD-2AD5-08D8878A18F1", ClaimType = "permission", ClaimValue = "Permission.Categories.View" },
            //                new IdentityRoleClaim<string> { Id = 15, RoleId = "8A02C605-8B6C-43DD-2AD5-08D8878A18F1", ClaimType = "permission", ClaimValue = "Permission.Categories.Create" },
            //                new IdentityRoleClaim<string> { Id = 16, RoleId = "8A02C605-8B6C-43DD-2AD5-08D8878A18F1", ClaimType = "permission", ClaimValue = "Permission.Categories.Edit" },
            //                new IdentityRoleClaim<string> { Id = 17, RoleId = "8A02C605-8B6C-43DD-2AD5-08D8878A18F1", ClaimType = "permission", ClaimValue = "Permission.Categories.Delete" },

            //                new IdentityRoleClaim<string> { Id = 18, RoleId = "8A02C605-8B6C-43DD-2AD5-08D8878A18F1", ClaimType = "permission", ClaimValue = "Permission.Products.View" },
            //                new IdentityRoleClaim<string> { Id = 19, RoleId = "8A02C605-8B6C-43DD-2AD5-08D8878A18F1", ClaimType = "permission", ClaimValue = "Permission.Products.Create" },
            //                new IdentityRoleClaim<string> { Id = 20, RoleId = "8A02C605-8B6C-43DD-2AD5-08D8878A18F1", ClaimType = "permission", ClaimValue = "Permission.Products.Edit" },
            //                new IdentityRoleClaim<string> { Id = 21, RoleId = "8A02C605-8B6C-43DD-2AD5-08D8878A18F1", ClaimType = "permission", ClaimValue = "Permission.Products.Delete" }
            //        );
        }
    }
}
