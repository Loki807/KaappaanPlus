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
    internal class AlertResponderRepository : IAlertResponderRepository
    {
        private readonly AppDbContext _context;

        public AlertResponderRepository(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Add new responder record (Police / Fire / Ambulance)
        public async Task<Guid> AddAsync(AlertResponder responder, CancellationToken ct = default)
        {
            await _context.AlertResponders.AddAsync(responder, ct);
            await _context.SaveChangesAsync(ct);
            return responder.Id;
        }

        // ✅ Get all responders linked to a specific Alert
        public async Task<IEnumerable<AlertResponder>> GetByAlertIdAsync(Guid alertId, CancellationToken ct = default)
        {
            return await _context.AlertResponders
                .Where(r => r.AlertId == alertId)
                .Include(r => r.AppUser)     // optional: to load responder user details
                .ToListAsync(ct);
        }
    }
}
