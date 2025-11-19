using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Domain.Entities;

using KaappanPlus.Persistence.Data;

namespace KaappaanPlus.Persistence.Repository
{
    public class LocationRepository : ILocationRepository
    {
        private readonly AppDbContext _context;

        public LocationRepository(AppDbContext context)
        {
            _context = context;
        }

        // ⭐ RESONDER LOCATION UPDATE
        public async Task SaveResponderLocationAsync(Guid responderId, double lat, double lng)
        {
            var log = new LocationLog(responderId, lat, lng);

            await _context.LocationLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        // ⭐ CITIZEN LOCATION UPDATE
        public async Task SaveCitizenLocationAsync(Guid citizenId, double lat, double lng)
        {
            var log = LocationLog.FromCitizen(citizenId, lat, lng);

            await _context.LocationLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}
