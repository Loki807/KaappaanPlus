using KaappaanPlus.Domain.Common;

namespace KaappaanPlus.Domain.Entities
{
    public class AlertResponder : AuditableEntity
    {
        public Guid AlertId { get; set; }
        public Alert Alert { get; set; } = default!;

        public Guid ResponderId { get; set; }
        public AppUser Responder { get; set; } = default!;

        public string AssignmentReason { get; set; }

        // ⭐ Added because your handler uses them
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public AlertResponder(Guid alertId, Guid responderId, string assignmentReason)
        {
            AlertId = alertId;
            ResponderId = responderId;
            AssignmentReason = assignmentReason;
        }
    }
}
