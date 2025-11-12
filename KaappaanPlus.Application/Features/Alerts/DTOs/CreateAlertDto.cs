using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.DTOs
{
    public class CreateAlertDto
    {
        public Guid CitizenId { get; set; }
        public string AlertTypeName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime? ReportedAt { get; set; } = DateTime.UtcNow;
    }
}
