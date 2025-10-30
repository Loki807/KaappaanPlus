using KaappaanPlus.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts.Persistence
{
    public interface IUserRepository
    {

        // ✅ Create
        Task CreateUserAsync(AppUser user, CancellationToken cancellationToken = default);

        // ✅ Read
        Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<AppUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<AppUser>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);

        // ✅ Update
        Task UpdateAsync(AppUser user, CancellationToken cancellationToken = default);

        // ✅ Delete
        Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
