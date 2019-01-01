using DMS.Data;
using DMS.Domain;
using DMS.Domain.ConfigSettings;
using DMS.Domain.Entities.Identity;
using DMS.Domain.Services;
using DMS.Domain.Services.Identity;
using DMS.Services;
using DMS.Services.Identity;
using DMS.WebApi.Authorization;
using DMS.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DMS.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = Configuration.GetSection("JwtSettings").Get<JwtSettings>();

            services.AddDbContext<DataContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString("DataContext")));

            services.AddIdentity<User, Role>()
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

            services.AddSingleton<IAuthorizationPolicyProvider, ClaimAuthorizationPolicyProvider>();

            services.AddScoped<IRequestContext, RequestContext>();
            services.AddScoped<IProjectsService, ProjectsService>();
            services.AddScoped<IIdentityService, IdentityService>();

            services.Configure<CorsSettings>(Configuration.GetSection("CorsSettings"));
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.Configure<SeedDataSettings>(Configuration.GetSection("SeedDataSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            IOptions<CorsSettings> corsSettings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseMiddleware<ErrorLoggingMiddleware>();

            app.UseCors(builder =>
                builder.WithOrigins(corsSettings.Value.Origin)
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseAuthentication();

            app.UseMiddleware<RequestContextResolveMiddleware>();

            app.UseMvc();
        }
    }
}
