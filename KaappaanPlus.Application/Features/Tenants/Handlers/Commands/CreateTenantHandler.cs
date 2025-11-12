using AutoMapper;
using KaappaanPlus.Application.Common.Exceptions;
using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Tenants.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

using Microsoft.Extensions.Logging;

namespace KaappaanPlus.Application.Features.Tenants.Handlers.Commands
{
    public class CreateTenantHandler : IRequestHandler<CreateTenantCommand, Guid>
    {
        private readonly ITenantRepository _tenantRepo;
        private readonly IRoleRepository _roleRepo;
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateTenantHandler(
            ITenantRepository tenantRepo,
            IRoleRepository roleRepo,
            IAppDbContext dbContext,
            IMapper mapper)
        {
            _tenantRepo = tenantRepo;
            _roleRepo = roleRepo;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            // ✅ Map DTO → Entity
            var tenant = _mapper.Map<Tenant>(request.TenantDto);

            // ✅ NEW: Include ServiceType (if not mapped automatically)
            tenant.ServiceType = request.TenantDto.ServiceType ?? "General";

            // ✅ Generate Code before saving
            tenant.Code = $"{tenant.City?.Substring(0, 3).ToUpper() ?? "TEN"}_TENANT";

            // ✅ Check duplicate
            var existingTenant = await _tenantRepo.GetByNameOrCityAsync(tenant.Name, tenant.City!, cancellationToken);
            if (existingTenant != null)
                throw new ConflictException("Tenant", $"'{tenant.Name}' already exists.");

            // ✅ Save
            var tenantId = await _tenantRepo.AddAsync(tenant, cancellationToken);

            // ✅ Auto-create TenantAdmin only if not exists
            var alreadyHasAdmin = await _tenantRepo.TenantAdminExistsAsync(tenantId, cancellationToken);
            if (!alreadyHasAdmin)
                await CreateTenantAdminAsync(tenant, cancellationToken);
            else
                return tenantId; // instead of _logger.LogWarning

            return tenantId;
        }

        private async Task CreateTenantAdminAsync(Tenant tenant, CancellationToken ct)
        {
            // 1️⃣ Get the TenantAdmin role
            var role = await _roleRepo.GetByNameAsync("TenantAdmin", ct);
            if (role == null)
            {
                // ⚠️ Role not found; skip admin creation
                throw new Exception($"Role 'TenantAdmin' not found for tenant {tenant.Name}");
            }

            // ✅ Clean email pattern
            var city = tenant.City?.ToLower().Replace(" ", "") ?? "tenant";
            var email = $"{city}_admin@kaappaan.com";

            // 3️⃣ Create the admin instance first
            var admin = new AppUser(
                tenant.Id,
                $"{tenant.Name} Admin",
                email,
                "0000000000",
                "",
                "TenantAdmin"
            );

            // 4️⃣ Hash password correctly
            var hasher = new PasswordHasher<AppUser>();
            var hashedPassword = hasher.HashPassword(admin, "Admin@123");
            admin.SetPasswordHash(hashedPassword);

            // 5️⃣ Assign role + flags
            admin.RequirePasswordChange();
            admin.UserRoles.Add(new UserRole(admin.Id, role.Id));

            // 6️⃣ Save to DB
            await _dbContext.AddEntityAsync(admin, ct);
            await _dbContext.SaveChangesAsync(ct);

            // ✅ Return confirmation instead of log
            return;
        }
    }
}
