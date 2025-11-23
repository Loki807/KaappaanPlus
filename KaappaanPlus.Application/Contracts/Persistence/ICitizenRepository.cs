using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KaappaanPlus.Domain.Entities;
namespace KaappaanPlus.Application.Contracts.Persistence
{
    public interface ICitizenRepository
    {
        Task<Citizen?> GetByAppUserIdAsync(Guid appUserId);
        Task AddAsync(Citizen citizen);
        Task<bool> ExistsByEmailAsync(string email);
        Task<IEnumerable<Citizen>> GetAllAsync();
        Task UpdateAsync(Citizen citizen);

        Task<Citizen?> GetByIdAsync(Guid id);

        Task DeleteAsync(Citizen citizen);
        Task<Citizen?> GetByUserIdAsync(Guid appUserId);


    }

}
