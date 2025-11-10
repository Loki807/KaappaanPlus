using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public class Alert : AuditableEntity
    {
       
        public Guid CitizenId { get; private set; }
        public Guid TenantId { get; private set; }


        public string Description { get; private set; } = default!;
        public string Location { get; private set; } = default!;
        public string Status { get; private set; } = "Pending";   // Pending / InProgress / Resolved

        public Guid AlertTypeId { get; private set; }
        public AlertType AlertTypeRef { get; private set; } = default!;
        // 📍 Incident location (from citizen)
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public ICollection<AlertResponder> Responders { get; private set; } = new List<AlertResponder>();

        private Alert() { }
        // optional
        public DateTime ReportedAt { get; private set; } = DateTime.UtcNow;

        // navigation
        public Citizen Citizen { get; private set; } = default!;
        public Tenant Tenant { get; private set; } = default!;



        public Alert(Guid citizenId, Guid tenantId, Guid alertTypeId, string? description, double lat, double lon)
        {
            CitizenId = citizenId;
            TenantId = tenantId;
            AlertTypeId = alertTypeId;
            Description = description;
            Latitude = lat;
            Longitude = lon;
            SetCreated("system");
        }


        public void UpdateStatus(string status)
        {
            Status = status;
            SetUpdated("system");
        }
    }
}
