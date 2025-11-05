using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts
{
    public interface IAppDbContext
    {
            IQueryable<Tenant> Tenants { get; }
            IQueryable<AppUser> AppUsers { get; }
            IQueryable<Role> Roles { get; }
            IQueryable<UserRole> UserRoles { get; }

            IQueryable<Alert> Alerts { get; }
            IQueryable<AlertResponder> AlertResponders { get; }
            IQueryable<LocationLog> LocationLogs { get; }


        Task AddEntityAsync<T>(T entity, CancellationToken cancellationToken = default) where T : class;

            Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);


            void UpdateEntity<T>(T entity) where T : class;
    }
       
    
}
