using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string? CreatedBy { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public string? UpdatedBy { get; private set; }

        public void SetCreated(string user)
        {
            CreatedBy = user;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetUpdated(string user)
        {
            UpdatedBy = user;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
