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
        public string AlertType { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Location { get; set; } = default!;
    }
}
