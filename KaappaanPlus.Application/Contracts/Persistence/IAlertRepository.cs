using KaappaanPlus.Application.Contracts.IBase;
using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts.Persistence
{
    public interface IAlertRepository
    {
        Task<Guid> AddAsync(Alert alert, CancellationToken ct = default);
        Task<IEnumerable<Alert>> GetByCitizenAsync(Guid citizenId, CancellationToken ct = default);
        Task<Alert?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task UpdateAsync(Alert alert, CancellationToken ct = default);
        Task DeleteAsync(Alert alert, CancellationToken ct = default);

        Task<IEnumerable<Alert>> GetAlertsByCitizenAsync(Guid citizenId, CancellationToken ct = default);

        Task<IEnumerable<Alert>> GetAlertsByTenantAsync(Guid tenantId, CancellationToken ct = default);


        Task<List<Alert>> GetAlertsByCitizenIdAsync(Guid citizenId, CancellationToken ct);
        Task<List<Alert>> GetAlertsByTenantIdAsync(Guid tenantId, CancellationToken ct);

    }
}

