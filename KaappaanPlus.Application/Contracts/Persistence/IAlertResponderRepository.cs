using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts.Persistence
{
    public interface IAlertResponderRepository
    {
        Task AddAsync(AlertResponder alertResponder, CancellationToken cancellationToken = default);


        Task<List<AlertResponder>> GetRespondersByAlertAsync(Guid alertId);
        Task<AlertResponder?> GetByAlertAndResponderAsync(Guid alertId, Guid responderId);
        Task UpdateAsync(AlertResponder entity);
        Task<AlertResponder?> GetByAlertAndResponderAsync(Guid alertId, Guid responderId, CancellationToken ct);
        Task<List<AlertResponder>> GetByAlertIdAsync(Guid alertId, CancellationToken ct);
        Task<List<AlertResponder>> GetByResponderIdAsync(Guid responderId, CancellationToken ct);
    }
}

