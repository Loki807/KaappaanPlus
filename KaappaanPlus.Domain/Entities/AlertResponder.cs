using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public class AlertResponder : AuditableEntity
    {
        public Guid AlertId { get; private set; }
        public Alert Alert { get; private set; } = default!;
       
       

       
    }
}
