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

        public async Task<Tenant?> GetByNameOrCityAsync(string name, string city, CancellationToken ct = default)
        {
            return await _context.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(t =>
                    t.Name.ToLower() == name.ToLower() ||
                    t.City!.ToLower() == city.ToLower(), ct);
        }

        public async Task<bool> TenantAdminExistsAsync(Guid tenantId, CancellationToken ct = default)
        {
            return await _context.AppUsers
                .AnyAsync(u => u.TenantId == tenantId && u.Role == "TenantAdmin", ct);
        }

        public async Task<Guid> AddAsync(Tenant tenant, CancellationToken ct = default)
        {
            await _context.Tenants.AddAsync(tenant, ct);
            await _context.SaveChangesAsync(ct);
            return tenant.Id;
        }

        public async Task<bool> ExistsAsync(Guid? tenantId, CancellationToken ct = default)
        {
            if (tenantId == null)
                return false;

            return await _context.Tenants.AnyAsync(t => t.Id == tenantId, ct);
        }
    }
}

