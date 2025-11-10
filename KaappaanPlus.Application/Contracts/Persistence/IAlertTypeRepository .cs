using KaappaanPlus.Application.Contracts.IBase;
using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts.Persistence
{
    public interface IAlertTypeRepository
    {
        Task<AlertType?> GetByNameAsync(string name, CancellationToken ct = default);
        Task AddAsync(AlertType alertType, CancellationToken ct = default);
    }
}
