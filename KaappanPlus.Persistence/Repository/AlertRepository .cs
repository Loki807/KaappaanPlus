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
    public class AlertRepository : IAlertRepository
    {
        private readonly AppDbContext _context;

        public AlertRepository(AppDbContext context)
        {
            _context = context;
        }

        // Add new alert
        public async Task<Guid> AddAsync(Alert alert, CancellationToken ct = default)
        {
            await _context.Alerts.AddAsync(alert, ct);
            await _context.SaveChangesAsync(ct);
            return alert.Id;
        }

        // Get all alerts for a citizen
        public async Task<IEnumerable<Alert>> GetByCitizenAsync(Guid citizenId, CancellationToken ct = default)
        {
            return await _context.Alerts
                .Include(a => a.AlertTypeRef)
                .Where(a => a.CitizenId == citizenId)
                .ToListAsync(ct);
        }

        // Get alert by Id
        public async Task<Alert?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Alerts
                .Include(a => a.AlertTypeRef)
                .FirstOrDefaultAsync(a => a.Id == id, ct);
        }

        // Update an alert
        public async Task UpdateAsync(Alert alert, CancellationToken ct = default)
        {
            _context.Alerts.Update(alert);
            await _context.SaveChangesAsync(ct);
        }

        // Delete an alert
        public async Task DeleteAsync(Alert alert, CancellationToken ct = default)
        {
            _context.Alerts.Remove(alert);
            await _context.SaveChangesAsync(ct);
        }

        // Get alerts by citizen
        public async Task<IEnumerable<Alert>> GetAlertsByCitizenAsync(Guid citizenId, CancellationToken ct = default)
        {
            return await _context.Alerts
                .Where(a => a.CitizenId == citizenId)
                .Include(a => a.AlertTypeRef) // You may want to include AlertType or other navigation properties
                .AsNoTracking()
                .ToListAsync(ct);
        }

        // Get alerts by tenant
        public async Task<IEnumerable<Alert>> GetAlertsByTenantAsync(Guid tenantId, CancellationToken ct = default)
        {
            return await _context.Alerts
                .Where(a => a.TenantId == tenantId)
                .Include(a => a.AlertTypeRef) // Include AlertType or any related data
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }
}
