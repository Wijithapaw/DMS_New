using DMS.Domain.Entities;
using DMS.Domain.Entities.Identity;
using DMS.Domain.Entities.System;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DMS.Domain
{
    public interface IDataContext : IDisposable
    {
        DbSet<Project> Projects { get; set; }
        DbSet<Donee> Donees { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Role> Roles { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<LookupHeader> LookupHeaders { get; set; }
        DbSet<Lookup> Lookups { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
