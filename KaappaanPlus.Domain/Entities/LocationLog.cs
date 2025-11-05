using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public class LocationLog : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public DateTime LoggedAt { get; private set; } = DateTime.UtcNow;

        public AppUser? User { get; private set; }
        // ✅ EF Core needs this
        private LocationLog() { }

        public LocationLog(Guid userId, double lat, double lng)
        {
            UserId = userId;
            Latitude = lat;
            Longitude = lng;
        }
    }
}
