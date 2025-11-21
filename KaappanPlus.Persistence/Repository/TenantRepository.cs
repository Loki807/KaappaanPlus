using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Domain.Entities;
using KaappanPlus.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappanPlus.Persistence.Repository
{
    public class TenantRepository : ITenantRepository
    {
        private readonly AppDbContext _context;

        public TenantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(Tenant tenant, CancellationToken ct = default)
        {
            await _context.Tenants.AddAsync(tenant, ct);
            await _context.SaveChangesAsync(ct);
            return tenant.Id;
        }

        public async Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Tenants.AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        public async Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Tenants
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ToListAsync(ct);
        }

        public async Task<Tenant?> GetByNameOrCityAsync(string name, string city, CancellationToken ct = default)
        {
            return await _context.Tenants
                .FirstOrDefaultAsync(t =>
                    t.Name.ToLower() == name.ToLower() ||
                    t.City.ToLower() == city.ToLower(), ct);
        }

        public async Task<bool> TenantAdminExistsAsync(Guid tenantId, CancellationToken ct = default)
        {
            return await _context.AppUsers
                .AnyAsync(u => u.TenantId == tenantId && u.Role == "TenantAdmin", ct);
        }

        public async Task UpdateAsync(Tenant tenant, CancellationToken ct = default)
        {
            _context.Tenants.Update(tenant);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var tenant = await _context.Tenants.FindAsync(new object[] { id }, ct);

            if (tenant != null)
            {
                var users = _context.AppUsers.Where(u => u.TenantId == id).ToList();
                foreach (var u in users)
                {
                    u.TenantId = null;
                }
                _context.Tenants.Remove(tenant);
                await _context.SaveChangesAsync();

            }
        }



        public async Task<bool> ExistsAsync(Guid? tenantId, CancellationToken ct = default)
        {
            if (tenantId == null) return false;
            return await _context.Tenants.AnyAsync(t => t.Id == tenantId.Value, ct);
        }

        public async Task<Tenant?> GetTenantByCityAndServiceAsync(string city, string serviceType, CancellationToken cancellationToken = default)
        {
            return await _context.Tenants
                .FirstOrDefaultAsync(t =>
                    t.City.ToLower() == city.ToLower() &&
                    t.ServiceType.ToLower() == serviceType.ToLower(),
                    cancellationToken);
        }

        public async Task<Tenant?> GetByCityAsync(string city, CancellationToken ct = default)
        {
            return await _context.Tenants.FirstOrDefaultAsync(t => t.City.ToLower() == city.ToLower(), ct);
        }

        public async Task<List<Tenant>> GetAllAsync()
        {
            return await _context.Tenants.ToListAsync();
        }


    }
}

