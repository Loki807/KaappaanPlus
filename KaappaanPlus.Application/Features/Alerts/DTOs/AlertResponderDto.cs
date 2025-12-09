using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.DTOs
{
    public class AlertResponderDto
    {
        public Guid ResponderId { get; set; }
        public string ResponderName { get; set; } = default!;
        public string Role { get; set; } = default!;
        public string AssignmentReason { get; set; } = default!;

        public string EmergencyContact { get; set; } = "";

    }
}
