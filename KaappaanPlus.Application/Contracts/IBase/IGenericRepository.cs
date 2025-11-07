using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Contracts.IBase
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<T?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}
