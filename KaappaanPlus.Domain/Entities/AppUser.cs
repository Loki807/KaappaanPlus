using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Domain.Entities
{
    public sealed class AppUser : AuditableEntity
    {
        public Guid TenantId { get; private set; }   // Multi-tenant system
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Phone { get; private set; } = default!;
        public string? PasswordHash { get; private set; }     // we NEVER store plain password

        public bool IsActive { get; private set; } = true;

        // Navigation
        public Tenant Tenant { get; private set; } = default!;
        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

        private AppUser() { }  // EF Core needs empty constructor

        public AppUser(Guid tenantId, string name, string email, string phone)
        {
            TenantId = tenantId;
            Name = name;
            Email = email;
            Phone = phone;

            SetCreated("system"); // temporary, later replace with logged-in user
        }

        //  Secure password setter
        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
            SetUpdated("system");
        }

        //  Update profile
        public void UpdateProfile(string name, string phone)
        {
            Name = name;
            Phone = phone;
            SetUpdated("system");
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}
//im nitha