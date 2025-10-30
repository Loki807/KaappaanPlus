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
        private readonly ILogger<CreateTenantHandler> _logger;

        public CreateTenantHandler(
            ITenantRepository tenantRepo,
            IRoleRepository roleRepo,
            IAppDbContext dbContext,
            IMapper mapper,
            ILogger<CreateTenantHandler> logger)
        {
            _tenantRepo = tenantRepo;
            _roleRepo = roleRepo;
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            // ✅ Map DTO → Entity
            var tenant = _mapper.Map<Tenant>(request.TenantDto);

            // ✅ Generate Code before saving
            tenant.Code = $"{tenant.City?.Substring(0, 3).ToUpper() ?? "TEN"}_TENANT"; // 👈 Add this line

            // ✅ Check duplicate
            var existingTenant = await _tenantRepo.GetByNameOrCityAsync(tenant.Name, tenant.City!, cancellationToken);
            if (existingTenant != null)
                throw new ConflictException("Tenant", $"'{tenant.Name}' already exists.");

            // ✅ Save
            var tenantId = await _tenantRepo.AddAsync(tenant, cancellationToken);
            _logger.LogInformation("✅ Tenant created: {TenantName}", tenant.Name);

            // ✅ Auto-create TenantAdmin only if not exists
            var alreadyHasAdmin = await _tenantRepo.TenantAdminExistsAsync(tenantId, cancellationToken);
            if (!alreadyHasAdmin)
                await CreateTenantAdminAsync(tenant, cancellationToken);
            else
                _logger.LogWarning("⚠️ TenantAdmin already exists for {TenantName}", tenant.Name);

            return tenantId;
        }



        private async Task CreateTenantAdminAsync(Tenant tenant, CancellationToken ct)
        {
            // 1️⃣ Get the TenantAdmin role
            var role = await _roleRepo.GetByNameAsync("TenantAdmin", ct);
            if (role == null)
            {
                _logger.LogWarning("⚠️ Role 'TenantAdmin' not found for {TenantName}", tenant.Name);
                return;
            }

            // ✅ Clean and simple email (no GUID)
            var city = tenant.City?.ToLower().Replace(" ", "") ?? "tenant";
            var email = $"{city}_admin@kaappaan.com";


            // 3️⃣ Create the admin instance first (empty password for now)
            var admin = new AppUser(
                tenant.Id,
                $"{tenant.Name} Admin",
                email,
                "0000000000",
                "", // temporarily empty password
                "TenantAdmin"
            );

            // 4️⃣ Hash the password correctly with the user instance
            var hasher = new PasswordHasher<AppUser>();
            var hashedPassword = hasher.HashPassword(admin, "Admin@123");
            admin.SetPasswordHash(hashedPassword);

            // 5️⃣ Set required flags and roles
            admin.RequirePasswordChange();
            admin.UserRoles.Add(new UserRole(admin.Id, role.Id));

            // 6️⃣ Save to DB
            await _dbContext.AddEntityAsync(admin, ct);
            await _dbContext.SaveChangesAsync(ct);

            // 7️⃣ Log credentials
            _logger.LogInformation("✅ TenantAdmin created for {TenantName}. Email: {Email}", tenant.Name, email);
            _logger.LogInformation("🔑 Default password: Admin@123");
        }

    }

    }
