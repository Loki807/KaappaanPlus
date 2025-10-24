using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users
{
    public class CreateUserDto
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string Role { get; set; } = default!;   // Allowed: TenantAdmin, Police, Fire, Traffic, Ambulance
        public Guid TenantId { get; set; }            // Must be an existing Tenant
    }
}
