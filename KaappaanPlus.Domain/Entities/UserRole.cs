using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public sealed class UserRole
    {
        public Guid AppUserId { get; private set; }  // FK → AppUser
        public Guid RoleId { get; private set; }     // FK → Role

        public AppUser AppUser { get; private set; } = default!;
        public Role Role { get; private set; } = default!;

        private UserRole() { }  // EF Core needs constructor

        public UserRole(Guid userId, Guid roleId)
        {
            AppUserId = userId;
            RoleId = roleId;
        }
    }
}
