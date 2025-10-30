using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappanPlus.Persistence.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        // ✅ EF Core actual DbSets
        public DbSet<Tenant> Tenants { get; set; } = default!;
        public DbSet<AppUser> AppUsers { get; set; } = default!;
        public DbSet<Role> Roles { get; set; } = default!;
        public DbSet<UserRole> UserRoles { get; set; } = default!;

        // ✅ Clean architecture returns as IQueryable automatically
        IQueryable<Tenant> IAppDbContext.Tenants => Tenants;
        IQueryable<AppUser> IAppDbContext.AppUsers => AppUsers;
        IQueryable<Role> IAppDbContext.Roles => Roles;
        IQueryable<UserRole> IAppDbContext.UserRoles => UserRoles;
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => base.SaveChangesAsync(cancellationToken);


        public void UpdateEntity<T>(T entity) where T : class
        {
            Set<T>().Update(entity);
        }
        public async Task AddEntityAsync<T>(T entity, CancellationToken cancellationToken = default)
     where T : class
        {
            await Set<T>().AddAsync(entity, cancellationToken);
        }

    }


}
