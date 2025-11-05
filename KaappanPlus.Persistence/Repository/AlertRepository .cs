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

        public AlertRepository(AppDbContext context) => _context = context;

        public async Task<Guid> AddAsync(Alert alert, CancellationToken ct = default)
        {
            await _context.Alerts.AddAsync(alert, ct);
            await _context.SaveChangesAsync(ct);
            return alert.Id;
        }

        public async Task<Alert?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _context.Alerts
                .Include(a => a.Responders)
                .FirstOrDefaultAsync(a => a.Id == id, ct);

        public async Task<IEnumerable<Alert>> GetAllAsync(CancellationToken ct = default)
            => await _context.Alerts.AsNoTracking().ToListAsync(ct);

        public async Task UpdateAsync(Alert alert, CancellationToken ct = default)
        {
            _context.Alerts.Update(alert);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var alert = await _context.Alerts.FindAsync(new object[] { id }, ct);
            if (alert != null)
            {
                _context.Alerts.Remove(alert);
                await _context.SaveChangesAsync(ct);
            }
        }
    }
}
