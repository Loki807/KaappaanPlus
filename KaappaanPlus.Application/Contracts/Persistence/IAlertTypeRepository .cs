using KaappaanPlus.Application.Contracts.IBase;
using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts.Persistence
{
    public interface IAlertTypeRepository : IGenericRepository<AlertType>
    {
        /// <summary>
        /// Get an alert type by its name (e.g. "Fire", "Accident").
        /// </summary>
        Task<AlertType?> GetByNameAsync(string name, CancellationToken ct = default);

        /// <summary>
        /// Get all active alert types.
        /// </summary>
        Task<List<AlertType>> GetActiveAsync(CancellationToken ct = default);


        Task<IEnumerable<AlertType>> GetAllAsync();
        Task<AlertType?> GetByNameAsync(string name);
    }
}
