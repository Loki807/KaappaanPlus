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
        // ✅ Add New Tenant
        Task<Guid> AddAsync(Tenant tenant, CancellationToken ct = default);

        // ✅ Get Tenant By ID
        Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct = default);

        // ✅ Get All Tenants
        Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken ct = default);

        // ✅ Get By Name or City (for Duplicate Check)
        Task<Tenant?> GetByNameOrCityAsync(string name, string city, CancellationToken ct = default);

        // ✅ Check if TenantAdmin Already Exists
        Task<bool> TenantAdminExistsAsync(Guid tenantId, CancellationToken ct = default);

        // ✅ Update Tenant
        Task UpdateAsync(Tenant tenant, CancellationToken ct = default);

        // ✅ Delete Tenant
        Task DeleteAsync(Guid id, CancellationToken ct = default);

        // ✅ Exists for validation or foreign key
        Task<bool> ExistsAsync(Guid? tenantId, CancellationToken ct = default);

        Task<Tenant?> GetByCityAsync(string city, CancellationToken ct = default);

        Task<Tenant?> GetTenantByCityAndServiceAsync(string city, string serviceType, CancellationToken cancellationToken = default);
    }
}
