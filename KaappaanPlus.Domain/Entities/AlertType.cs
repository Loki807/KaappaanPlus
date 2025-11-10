using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public class AlertType : BaseEntity
    {
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;

        // ServiceType to classify the AlertType
        public ServiceType Service { get; private set; }

        public AlertType(string name, string description, ServiceType service)
        {
            Name = name;
            Description = description;
            Service = service;
        }
    }
}
