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
    public class AlertTypeRepository : GenericRepository<AlertType>, IAlertTypeRepository
    {
        private readonly AppDbContext _db;

        public AlertTypeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        // ✅ Get alert type by name
        public async Task<AlertType?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            return await _db.AlertTypes
                .Where(a => a.Name.ToLower() == name.ToLower() && a.IsActive)
                .FirstOrDefaultAsync(ct);
        }

        // ✅ Get all active types
        public async Task<List<AlertType>> GetActiveAsync(CancellationToken ct = default)
        {
            return await _db.AlertTypes
                .Where(a => a.IsActive)
                .OrderBy(a => a.Name)
                .ToListAsync(ct);
        }

        public async Task<AlertType?> GetByNameAsync(string name)
        {
            return await _db.AlertTypes
                .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
        }

        public async Task<IEnumerable<AlertType>> GetAllAsync()
        {
            return await _db.AlertTypes.OrderBy(t => t.Name).ToListAsync();
        }
    }
}
