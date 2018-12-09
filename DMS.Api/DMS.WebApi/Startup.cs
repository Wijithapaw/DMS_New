using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DMS.Domain.Services;
using DMS.Services;
using DMS.Domain;
using DMS.Data;
using Microsoft.EntityFrameworkCore;
using DMS.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DMS.WebApi.ErrorHandling;
using Microsoft.AspNetCore.Http;
using DMS.Utills;
using DMS.WebApi.Utills;
using DMS.Utills.CustomClaims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using System.Net;
using DMS.WebApi.AuthRequirements;
using Microsoft.AspNetCore.Authorization;
using DMS.Utills.ConfigSettings;
using Microsoft.Extensions.Options;

namespace DMS.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = Configuration.GetSection("JwtSettings").Get<JwtSettings>();

            services.AddDbContext<DataContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DataContext")));

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Audience = jwtSettings.Audience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSettings.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey))
                };
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = ctx =>
                {
                   ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                   return Task.FromResult(0);
                };

                options.Events.OnRedirectToAccessDenied = ctx =>
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return Task.FromResult(0);
                };
            });

            // Add framework services.
            services.AddMvc();

            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ManageProjects",
                    policy => policy.Requirements.Add(new ManageProjectRequirement()));

                options.AddPolicy("ManageAccounts",
                    policy => policy.Requirements.Add(new ManageAccountsRequirment()));

                options.AddPolicy("ManageSystemSettings",
                   policy => policy.Requirements.Add(new ManageSystemSettingsRequirment()));
            });

            services.AddSingleton<IAuthorizationHandler, ManageAccountsHandler>();
            services.AddSingleton<IAuthorizationHandler, ManageSystemSettingsHandler>();
            services.AddSingleton<IAuthorizationHandler, ManageProjectHandler>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IEnvironmentDescriptor, WebEnvironmentDescriptor>();
            services.AddTransient<IProjectsService, ProjectsService>();
            services.AddTransient<IProjectCategoryService, ProjectCategoryService>();
            services.AddTransient<IAccountService, AccountService>();

            services.Configure<CorsSettings>(Configuration.GetSection("CorsSettings"));
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory, 
            DataContext context, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<ApplicationRole> roleManager,
            IOptions<CorsSettings> corsSettings)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCors(builder =>
                builder.WithOrigins(corsSettings.Value.Origin)
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseAuthentication();

            app.UseMvc();

            //Apply any pending migrations
            DbInitializer.Initialize(context, userManager, roleManager);
        }
    }
}
