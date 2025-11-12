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
    public class AlertResponderRepository : IAlertResponderRepository
    {
        private readonly IAppDbContext _context;

        public AlertResponderRepository(IAppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AlertResponder alertResponder, CancellationToken cancellationToken = default)
        {
            await _context.AddEntityAsync(alertResponder, cancellationToken);
        }

        public async Task<List<AlertResponder>> GetByAlertIdAsync(Guid alertId, CancellationToken ct)
        {
            return await _context.AlertResponders
                .Include(ar => ar.Alert)      // Include related alert info
                .Include(ar => ar.Responder)  // Include responder (AppUser)
                .Where(ar => ar.AlertId == alertId)
                .ToListAsync(ct);
        }
    }
}
