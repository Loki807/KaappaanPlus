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
        Task<Guid> AddAsync(AlertResponder responder, CancellationToken ct = default);
    }
}

