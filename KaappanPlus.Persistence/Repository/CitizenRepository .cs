using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Domain.Entities;
using KaappanPlus.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappanPlus.Persistence.Repository
{
    public class CitizenRepository : ICitizenRepository
    {
        private readonly AppDbContext _context;

        public CitizenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Citizen citizen)
        {
            await _context.Citizens.AddAsync(citizen);
            await _context.SaveChangesAsync();
        }

       
        public Task<bool> ExistsByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Citizen>> GetAllAsync()
        {
            return await _context.Citizens
                .Include(c => c.AppUser)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Citizen?> GetByAppUserIdAsync(Guid appUserId)
        {
            return await _context.Citizens
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(c => c.AppUserId == appUserId);
        }

        public async Task<Citizen?> GetByIdAsync(Guid id)
              => await _context.Citizens.FirstOrDefaultAsync(c => c.Id == id);

        public async Task UpdateAsync(Citizen citizen)
        {
            _context.Citizens.Update(citizen);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Citizen citizen)
        {
            _context.Citizens.Remove(citizen);
            await _context.SaveChangesAsync();
     
        
        }


    }


}
