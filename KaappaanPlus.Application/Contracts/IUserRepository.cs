using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task CreateUserAsync(AppUser user, CancellationToken cancellationToken = default);

        
        Task<AppUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
       
        Task UpdateAsync(AppUser user, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}

