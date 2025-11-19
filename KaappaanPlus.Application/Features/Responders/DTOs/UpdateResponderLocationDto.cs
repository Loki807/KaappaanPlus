using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Responders.DTOs
{
    public class UpdateResponderLocationDto
    {
        public Guid ResponderId { get; set; }
        public Guid AlertId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}
