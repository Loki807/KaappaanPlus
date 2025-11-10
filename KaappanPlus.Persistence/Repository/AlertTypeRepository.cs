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
    public class AlertTypeRepository : IAlertTypeRepository
    {
        private readonly AppDbContext _context;

        public AlertTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get alert type by name
        public async Task<AlertType?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            return await _context.AlertTypes
                .FirstOrDefaultAsync(a => a.Name == name, ct);
        }

        // Add new alert type
        public async Task AddAsync(AlertType alertType, CancellationToken ct = default)
        {
            await _context.AlertTypes.AddAsync(alertType, ct);
            await _context.SaveChangesAsync(ct);
        }
    }
}
