using KaappaanPlus.Application.Contracts;
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
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Create
        public async Task CreateUserAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            await _context.AppUsers.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // ✅ Get by email
        public async Task<AppUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.AppUsers
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), cancellationToken);
        }

        // ✅ Get by ID
        public async Task<AppUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.AppUsers
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        // ✅ Get all users by TenantId
        public async Task<IEnumerable<AppUser>> GetByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            return await _context.AppUsers
                .Where(u => u.TenantId == tenantId)
                .Include(u => u.UserRoles)
                .ToListAsync(cancellationToken);
        }

        // ✅ Update user
        public async Task UpdateAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            var existing = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
            if (existing == null)
                throw new KeyNotFoundException($"User with ID {user.Id} not found");

            existing.UpdateInfo(user.Name, user.Phone, user.Role, user.IsActive);
            _context.AppUsers.Update(existing);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // ✅ Delete user
        public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var user = await _context.AppUsers.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found");

            _context.AppUsers.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(AppUser user)
        {
            _context.AppUsers.Remove(user);
            await _context.SaveChangesAsync();
        }

        public Task<List<AppUser>> GetRespondersByCityAndRolesAsync(string city, IEnumerable<string> roles, CancellationToken ct = default)
        {
            return _context.AppUsers
                .Include(u => u.Tenant)
                .Where(u => roles.Contains(u.Role!) &&
                            u.Tenant.City == city &&
                            u.Tenant.IsActive)
                .ToListAsync(ct);
        }

    }
}



    //lk

    //lk how are you


