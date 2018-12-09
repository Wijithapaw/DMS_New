using Microsoft.EntityFrameworkCore;
using DMS.Data.Entities;
using System.Linq;
using System;
using DMS.Utills;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DMS.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        IEnvironmentDescriptor _env;

        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {
            
        }

        public DataContext(DbContextOptions<DataContext> options, IEnvironmentDescriptor env) : base(options)
        {
            _env = env;
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectCategory> ProjectCategories { get; set; }

        public DbSet<Donee> Donees { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ProjectCategory>()
                .HasIndex(c => c.Title)
                .IsUnique(true);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_env != null)
            {
                foreach (var entry in this.ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Added))
                {
                    entry.Entity.CreatedBy = _env.UserId;
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                }

                foreach (var entry in this.ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Modified))
                {
                    entry.Entity.LastUpdatedBy = _env.UserId;
                    entry.Entity.LastUpdatedDate = DateTime.UtcNow;

                    entry.Property("RowVersion").OriginalValue = entry.Entity.RowVersion;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            if (_env != null)
            {
                foreach (var entry in this.ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Added))
                {
                    entry.Entity.CreatedBy  = _env.UserId;
                    entry.Entity.CreatedDate  = DateTime.UtcNow;
                }

                foreach (var entry in this.ChangeTracker.Entries<BaseEntity>().Where(e => e.State == EntityState.Modified))
                {
                    entry.Entity.LastUpdatedBy = _env.UserId;
                    entry.Entity.LastUpdatedDate = DateTime.UtcNow;

                    entry.Property("RowVersion").OriginalValue = entry.Entity.RowVersion;
                }
            }
            return base.SaveChanges();
        }

    }
}
