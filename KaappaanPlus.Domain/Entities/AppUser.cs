using KaappaanPlus.Domain.Common;
using System;
using System.Collections.Generic;

namespace KaappaanPlus.Domain.Entities
{
    public sealed class AppUser : AuditableEntity
    {
        public Guid? TenantId { get; private set; }          // Multi-tenant system
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Phone { get; private set; } = default!;
        public string? PasswordHash { get; private set; }    // never store plain password
        public string Role { get; private set; } = default!;
        public bool IsActive { get;  set; } = true;

        // Navigation
        public Tenant Tenant { get; private set; } = default!;
        public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

        // EF Core needs an empty constructor → keep it protected
        private AppUser() { }

        // ✅ Basic constructor (without password)
        public AppUser(Guid tenantId, string name, string email, string phone)
        {
            TenantId = tenantId;
            Name = name;
            Email = email;
            Phone = phone;
            SetCreated("system");
        }

        // ✅ Full constructor (used for registration or seeding)
        public AppUser(Guid tenantId, string name, string email, string phone, string passwordHash, string role)
        {
            TenantId = tenantId;
            Name = name;
            Email = email;
            Phone = phone;
            PasswordHash = passwordHash;
            Role = role;
            IsActive = true;
            MustChangePassword = false; // normal users don't need to change immediately
            SetCreated("system");
        }

        // ✅ Secure password setter
        public void SetPasswordHash(string passwordHash)
        {
            PasswordHash = passwordHash;
            SetUpdated("system");
        }

        // ✅ Profile update
        public void UpdateProfile(string name, string phone)
        {
            Name = name;
            Phone = phone;
            SetUpdated("system");
        }

        // ✅ Activation control
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;

        // ✅ Password change rules
        public bool MustChangePassword { get; private set; } = false;

        public void RequirePasswordChange() => MustChangePassword = true;
        public void ClearPasswordChangeRequirement() => MustChangePassword = false;

        public void UpdatePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
            ClearPasswordChangeRequirement(); // remove flag after change
            SetUpdated("system");
        }

        // ✅ Generic user info update
        public void UpdateInfo(string name, string phone, string role, bool isActive)
        {
            Name = name;
            Phone = phone;
            Role = role;
            IsActive = isActive;
            SetUpdated("system");
        }
        public bool IsFirstLogin { get; set; } = true;
    }
}
