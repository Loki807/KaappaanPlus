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
        public string AlertTypeName { get; set; } = default!;   // e.g. "Fire", "Accident"
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
