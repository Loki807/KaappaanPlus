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
        public Guid? UserId { get;  set; }      // Responder
        public Guid? CitizenId { get;  set; }   // Citizen

        public double Latitude { get; set; }
        public double Longitude { get;  set; }
        public DateTime LoggedAt { get;  set; } = DateTime.UtcNow;

        public AppUser? User { get;  set; }
        public Citizen? Citizen { get;  set; }

        // EF Core required
        private LocationLog() { }

        // For Responder tracking
        public LocationLog(Guid userId, double lat, double lng)
        {
            UserId = userId;
            Latitude = lat;
            Longitude = lng;
        }

        // For Citizen tracking
        public static LocationLog FromCitizen(Guid citizenId, double lat, double lng)
        {
            return new LocationLog
            {
                CitizenId = citizenId,
                Latitude = lat,
                Longitude = lng,
                LoggedAt = DateTime.UtcNow
            };
        }
    }

}
