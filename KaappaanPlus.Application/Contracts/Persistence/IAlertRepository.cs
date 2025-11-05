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
        Task<Alert?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<IEnumerable<Alert>> GetAllAsync(CancellationToken ct = default);
        Task UpdateAsync(Alert alert, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
