using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.DTOs
{
    public class CreateAlertDto
    {
        public Guid TenantId { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string Type { get; set; } = default!;
        public string? Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
