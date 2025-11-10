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
    public class AlertResponderRepository : IAlertResponderRepository
    {
        private readonly AppDbContext _context;

        public AlertResponderRepository(AppDbContext context)
        {
            _context = context;
        }

        // Add a new responder to an alert
        public async Task<Guid> AddAsync(AlertResponder responder, CancellationToken ct = default)
        {
            await _context.AlertResponders.AddAsync(responder, ct);
            await _context.SaveChangesAsync(ct);
            return responder.Id;
        }

        // Get all responders by Alert ID
        public async Task<IEnumerable<AlertResponder>> GetByAlertIdAsync(Guid alertId, CancellationToken ct = default)
        {
            return await _context.AlertResponders
                .Include(r => r.Responder) // Include responder details (AppUser)
                .Where(r => r.AlertId == alertId)
                .ToListAsync(ct);
        }
    }
}
