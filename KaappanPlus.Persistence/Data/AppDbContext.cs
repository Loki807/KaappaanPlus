using Microsoft.EntityFrameworkCore;
using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Domain.Entities;
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
        public DbSet<Alert> Alerts { get; set; } = default!;
        public DbSet<AlertResponder> AlertResponders { get; set; } = default!;
        public DbSet<LocationLog> LocationLogs { get; set; } = default!;
        public DbSet<AlertType> AlertTypes { get; set; } = default!;
        
        public DbSet<Citizen> Citizens { get; set; } = default!;
        IQueryable<Citizen> IAppDbContext.Citizens => Citizens;
        IQueryable<Alert> IAppDbContext.Alerts => Alerts;
        IQueryable<AlertResponder> IAppDbContext.AlertResponders => AlertResponders;
        IQueryable<LocationLog> IAppDbContext.LocationLogs => LocationLogs;

        // ✅ Clean architecture returns as IQueryable automatically
        IQueryable<Tenant> IAppDbContext.Tenants => Tenants;
        IQueryable<AppUser> IAppDbContext.AppUsers => AppUsers;
        IQueryable<Role> IAppDbContext.Roles => Roles;
        IQueryable<UserRole> IAppDbContext.UserRoles => UserRoles;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);


            // Modify delete behavior for Alert and AppUser relationships in AlertResponder
            modelBuilder.Entity<AlertResponder>()
                .HasOne(ar => ar.Alert)
                .WithMany() // Assuming no reverse navigation property
                .HasForeignKey(ar => ar.AlertId)
                .OnDelete(DeleteBehavior.Restrict); // Set to Restrict instead of Cascade

            modelBuilder.Entity<AlertResponder>()
                .HasOne(ar => ar.Responder)
                .WithMany() // Assuming no reverse navigation property
                .HasForeignKey(ar => ar.ResponderId)
                .OnDelete(DeleteBehavior.Restrict); // Set to Restrict instead of Cascade

            base.OnModelCreating(modelBuilder);  // This line is already there, no need to call it again.



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

        public async Task<T> FindAsync<T>(Guid id, CancellationToken cancellationToken = default) where T : class
        {
            return await Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

    }


}
