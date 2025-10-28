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

        // ✅ Check if tenant exists by ID
        public async Task<bool> ExistsAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            // Handle invalid Guid safely
            if (tenantId == Guid.Empty)
                return false;

            return await _context.Tenants
                .AnyAsync(t => t.Id == tenantId, cancellationToken);
        }

        public Task<bool> ExistsAsync(Guid? tenantId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
