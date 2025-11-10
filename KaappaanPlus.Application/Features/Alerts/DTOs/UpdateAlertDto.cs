using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.DTOs
{
    public class UpdateAlertDto
    {
        public Guid Id { get; set; }               // The ID of the alert to update
        public string? Status { get; set; }         // New status for the alert
        public string? Description { get; set; }    // New description
        public string? AlertTypeName { get; set; }  // New alert type name
        public double Latitude { get; set; }        // New latitude
        public double Longitude { get; set; }       // New longitude
    }
}
