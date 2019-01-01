using DMS.Domain;
using DMS.Domain.Entities;
using DMS.Domain.Entities.Identity;
using DMS.Domain.Entities.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DMS.Domain.Entities.System;
using System.Linq.Expressions;
using Remotion.Linq.Parsing.ExpressionVisitors;

namespace DMS.Data
{
    public class DataContext : IdentityDbContext<User, Role, int>, IDataContext
    {
        IRequestContext _requestContext;

        public DataContext(DbContextOptions<DataContext> options) : base (options) { }

        public DataContext(DbContextOptions<DataContext> options, IRequestContext requestContext) : base(options)
        {
            _requestContext = requestContext;
        }

        #region Entities

        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<LookupHeader> LookupHeaders { get; set; }
        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Donee> Donees { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Model.GetEntityTypes()
                .Where(entityType => typeof(ISoftDeletableEntity).IsAssignableFrom(entityType.ClrType))
                .ToList()
                .ForEach(entityType =>
                {
                    builder.Entity(entityType.ClrType)
                    .HasQueryFilter(ConvertFilterExpression<ISoftDeletableEntity>(e => e.Deleted == 0, entityType.ClrType));
                });

            builder.Entity<LookupHeader>()
                .HasIndex(c =>  new { c.Code, c.Deleted })
                .IsUnique(true);

            builder.Entity<Lookup>()
                .HasIndex(c => new { c.HeaderId, c.Code, c.Deleted })
                .IsUnique(true);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_requestContext != null)
            {
                foreach (var entry in ChangeTracker.Entries<IAuditedEntity>().Where(e => e.State == EntityState.Added))
                {
                    entry.Entity.CreatedBy = entry.Entity.LastUpdatedBy = _requestContext.UserId;
                    entry.Entity.CreatedDateUtc = entry.Entity.LastUpdatedDateUtc = DateTime.UtcNow;
                }

                foreach (var entry in ChangeTracker.Entries<IAuditedEntity>().Where(e => e.State == EntityState.Modified))
                {
                    entry.Entity.LastUpdatedBy = _requestContext.UserId;
                    entry.Entity.LastUpdatedDateUtc = DateTime.UtcNow;
                }

                foreach (var entry in ChangeTracker.Entries<IConcurrencyHandledEntity>().Where(e => e.State == EntityState.Modified))
                {
                    entry.Property("RowVersion").OriginalValue = entry.Entity.RowVersion;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private static LambdaExpression ConvertFilterExpression<TInterface>(
                            Expression<Func<TInterface, bool>> filterExpression,
                            Type entityType)
        {
            var newParam = Expression.Parameter(entityType);
            var newBody = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), newParam, filterExpression.Body);

            return Expression.Lambda(newBody, newParam);
        }
    }
}
