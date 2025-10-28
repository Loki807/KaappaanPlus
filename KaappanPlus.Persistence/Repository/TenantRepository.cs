using KaappaanPlus.Application.Contracts.Persistence;
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

        public async Task<bool> ExistsAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            return await _context.Tenants.AnyAsync(t => t.Id == tenantId, cancellationToken);
        }
    }
}
