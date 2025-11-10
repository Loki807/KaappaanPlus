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
    public class AlertRepository : GenericRepository<Alert>, IAlertRepository
    {
        private readonly AppDbContext _alertContext;
        public AlertRepository(AppDbContext context) : base(context)
        {
            _alertContext = context;
        }

        public async Task<List<Alert>> GetAlertsByTenantAsync(Guid tenantId)
        {
            return await _context.Alerts
                .Include(a => a.Citizen)
                .ThenInclude(c => c.AppUser)
                .Include(a => a.Tenant)
                .Where(a => a.TenantId == tenantId)
                .OrderByDescending(a => a.ReportedAt)
                .ToListAsync();
        }

        public async Task<List<Alert>> GetAlertsByCitizenAsync(Guid citizenId)
        {
            return await _context.Alerts
                .Include(a => a.Citizen)
                .ThenInclude(c => c.AppUser)
                .Include(a => a.Tenant)
                .Where(a => a.CitizenId == citizenId)
                .OrderByDescending(a => a.ReportedAt)
                .ToListAsync();
        }

        public async Task<List<Alert>> GetAlertsByStatusAsync(Guid tenantId, string status)
        {
            return await _context.Alerts
                .Include(a => a.Citizen)
                .ThenInclude(c => c.AppUser)
                .Include(a => a.Tenant)
                .Where(a => a.TenantId == tenantId && a.Status == status)
                .OrderByDescending(a => a.ReportedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Alert>> GetByTenantAsync(Guid tenantId)
        {
            return await _context.Alerts
                .Include(a => a.AlertTypeRef)
                .Include(a => a.Citizen)
                .Where(a => a.TenantId == tenantId)
                .OrderByDescending(a => a.ReportedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Alert>> GetByCitizenAsync(Guid citizenId)
        {
            return await _context.Alerts
                .Include(a => a.AlertTypeRef)
                .Where(a => a.CitizenId == citizenId)
                .OrderByDescending(a => a.ReportedAt)
                .ToListAsync();
        }

        public async Task<Alert?> GetByIdAsync(Guid id)
        {
            return await _context.Alerts
                .Include(a => a.AlertTypeRef)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Alert>> GetActiveAlertsAsync(Guid tenantId)
        {
            return await _context.Alerts
                .Where(a => a.TenantId == tenantId && a.Status != "Resolved")
                .ToListAsync();
        }

        public Task<AlertType?> GetByNameAsync(string name, CancellationToken ct = default)
           => _alertContext.AlertTypes.FirstOrDefaultAsync(x => x.Name == name, ct);
    }
}
