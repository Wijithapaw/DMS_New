using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using DMS.Data;
using Microsoft.AspNetCore.Identity;
using DMS.Domain.Entities.Identity;
using NLog.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DMS.Domain.ConfigSettings;

namespace DMS.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args);

            SeedData(host);

            host.Run();
        }

        public static IWebHost CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .Build();

        private  static void SeedData(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<DataContext>();
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();
                var seedDataSettings = scope.ServiceProvider.GetService<IOptions<SeedDataSettings>>();

                DbInitializer.SeedDataAsync(dbContext, userManager, roleManager, seedDataSettings.Value).Wait();
            }
        }
    }
}
