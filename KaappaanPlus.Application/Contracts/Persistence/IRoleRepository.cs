using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts.Persistence
{
    public interface IRoleRepository
    {
        Task<Role?> GetByNameAsync(string roleName, CancellationToken cancellationToken);
    }
}
