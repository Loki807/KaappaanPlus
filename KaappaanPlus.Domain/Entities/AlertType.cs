using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public sealed class AlertType : AuditableEntity
    {
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }
        public bool IsActive { get; private set; } = true;

        private AlertType() { }

        public AlertType(string name, string? description = null)
        {
            Name = name;
            Description = description;
            IsActive = true;
            SetCreated("system");
        }

        public void Deactivate()
        {
            IsActive = false;
            SetUpdated("system");
        }
    }
}
