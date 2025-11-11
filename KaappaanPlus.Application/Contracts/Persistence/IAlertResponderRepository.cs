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
    }
}

