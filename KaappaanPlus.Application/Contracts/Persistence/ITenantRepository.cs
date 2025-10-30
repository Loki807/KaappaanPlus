using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts.Persistence
{
    public interface ITenantRepository
    {
        Task<Tenant?> GetByNameOrCityAsync(string name, string city, CancellationToken ct = default);
        Task<bool> TenantAdminExistsAsync(Guid tenantId, CancellationToken ct = default);
        Task<Guid> AddAsync(Tenant tenant, CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid? tenantId, CancellationToken ct = default);
    }
}
