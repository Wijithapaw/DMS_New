using DMS.Data.Entities;
using DMS.Utills.CustomClaims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace DMS.Data
{
    public static class DbInitializer
    {
        public async static void Initialize(DataContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            //Comment out this section once the initial phase of development is done
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //Uncomment this section once the initial phase of development is done
            //context.Database.Migrate();

            if (context.Users.Any())
                return;

            var adminRole = new ApplicationRole("Admin");
            var donorRole = new ApplicationRole("Donor");

            var roleResult = await roleManager.CreateAsync(adminRole);
            await roleManager.CreateAsync(donorRole);
            
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "projects.view"));
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "projects.edit"));
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "projects.create"));
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "system.settings.manage"));
            await roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, "accounts.manage"));
            await roleManager.AddClaimAsync(donorRole, new Claim(CustomClaimTypes.Permission, "projects.view"));
            await roleManager.AddClaimAsync(donorRole, new Claim(CustomClaimTypes.Permission, "projects.create"));

            var adminUser = new ApplicationUser
            {
                Email = "wijitha@outlook.com",
                FirstName = "Super",
                LastName = "Admin",
                Birthday = new DateTime(1985, 8, 1),
                Active = true,
                UserName = "wijitha@outlook.com",
                CreatedBy = 1,
                CreatedDate = DateTime.Now,
                LastUpdatedBy = 1,
                LastUpdatedDate = DateTime.Now
            };

            var userResult = await userManager.CreateAsync(adminUser, "Pwd123");

            if (userResult.Succeeded && roleResult.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");

                var donees = new Donee[]
                    {
                    new Donee{ FirstName = "Dinith", LastName = "Atapattu", Birthday = new DateTime(1992,8,1), Active=true, Email="dinith.dms@yopmail.com" },
                    new Donee{ FirstName = "Wishwa", LastName = "Atapattu", Birthday = new DateTime(1995,8,1), Active=true, Email="wishawa.dms@yopmail.com" },
                    new Donee{ FirstName = "Ranjith", LastName = "Gamage", Birthday = new DateTime(1992,8,5), Active=true, Email="ranjith.dms@yopmail.com" }
                    };

                context.Donees.AddRange(donees);
                context.SaveChanges();

                var projectCategories = new ProjectCategory[]
                   {
                    new ProjectCategory { Title = "Educational", Description="Scholarship for educational purpose"},
                    new ProjectCategory { Title = "Self Employment", Description="Financial aid for develop self-employment"},
                    new ProjectCategory { Title = "Other", Description="Other financial aid"}
                   };

                context.ProjectCategories.AddRange(projectCategories);
                context.SaveChanges();

                var projects = new Project[]
                    {
                    new Project { Description="Scholorship to University student", ProjectOwnerId = adminUser.Id, ProjectCategoryId=1, EndDate= new DateTime(2019,1,5), StartDate = new DateTime(2017, 4, 1), Title="Scholarhip to Saman"},
                    new Project { Description="Scholorship to A/L student",  ProjectOwnerId = adminUser.Id, ProjectCategoryId=1, EndDate= new DateTime(2018,1,5), StartDate = new DateTime(2017,7, 1), Title="Schorship to Madura"},
                    new Project { Description="Scholorship to O/L student", ProjectOwnerId = adminUser.Id, ProjectCategoryId=1, EndDate= new DateTime(2017,12,31), StartDate = new DateTime(2017, 4, 1), Title="Scholarhip to Kasun"}
                    };

                context.Projects.AddRange(projects);
                context.SaveChanges();
            }

            var donorUser = new ApplicationUser
            {
                Email = "widura@outlook.com",
                FirstName = "Widura",
                LastName = "Wijenayake",
                Birthday = new DateTime(1990, 8, 1),
                Active = true,
                UserName = "widura@outlook.com",
                CreatedBy = 1,
                CreatedDate = DateTime.Now,
                LastUpdatedBy = 1,
                LastUpdatedDate = DateTime.Now
            };

            await userManager.CreateAsync(donorUser, "Pwd123");
            await userManager.AddToRoleAsync(donorUser, "Donor");
        }
    }
}
