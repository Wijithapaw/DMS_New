using DMS.Data;
using DMS.Domain.ConfigSettings;
using DMS.Domain.Constants;
using DMS.Domain.Entities;
using DMS.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.WebApi
{
    public static class DbInitializer
    {
        public async static Task SeedDataAsync(DataContext context, 
            UserManager<User> userManager, 
            RoleManager<Role> roleManager,
            SeedDataSettings  seedDataSettings)
        {
            //Comment out this section once the initial phase of development is done
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            //Uncomment this section once the initial phase of development is done
            //context.Database.Migrate();

            if (context.Users.Any())
                return;

            var superAdminRole = new Role("Super Admin");

            await roleManager.CreateAsync(superAdminRole);

            var adminUser = new User
            {
                Email = seedDataSettings.SuperAdmin.Email,
                FirstName = seedDataSettings.SuperAdmin.FirstName,
                LastName = seedDataSettings.SuperAdmin.LastName,
                Birthday = new DateTime(1985, 8, 1),
                Active = true,
                UserName = seedDataSettings.SuperAdmin.Email,
                NormalizedEmail = seedDataSettings.SuperAdmin.Email.ToUpper(),
                NormalizedUserName = seedDataSettings.SuperAdmin.Email.ToUpper(),
                CreatedBy = 1,
                CreatedDateUtc = DateTime.Now,
                LastUpdatedBy = 1,
                LastUpdatedDateUtc = DateTime.Now
            };

            await userManager.CreateAsync(adminUser, seedDataSettings.SuperAdmin.Password);

            await userManager.AddToRoleAsync(adminUser, "Super Admin");
        }
    }
}
