using AutoMapper;
using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Tenants.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KaappaanPlus.Application.Features.Tenants.Handlers.Commands
{
    public class CreateTenantHandler : IRequestHandler<CreateTenantCommand, Guid>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IRoleRepository _roleRepo;
        private readonly ILogger<CreateTenantHandler> _logger;
        private readonly IMapper _mapper;

        public CreateTenantHandler(IAppDbContext dbContext, IRoleRepository roleRepo, IMapper mapper, ILogger<CreateTenantHandler> logger)
        {
            _dbContext = dbContext;
            _roleRepo = roleRepo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Guid> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            // ✅ 1) Map DTO → Tenant entity
            var tenant = _mapper.Map<Tenant>(request.TenantDto);

            // ✅ 2) Generate Tenant Code
            var shortPrefix = request.TenantDto.City?.Substring(0, 3).ToUpper() ?? "TEN";
            tenant.GetType().GetProperty("Code")?.SetValue(tenant, $"{shortPrefix}_TENANT");

            // ✅ 3) Save Tenant to DB
            await _dbContext.AddEntityAsync(tenant, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("✅ Tenant created: {TenantName} ({TenantCode})", tenant.Name, tenant.Code);

            // ✅ 4) Auto-create Tenant Admin
            await CreateTenantAdminAsync(tenant, cancellationToken);

            return tenant.Id;
        }

        private async Task CreateTenantAdminAsync(Tenant tenant, CancellationToken cancellationToken)
        {
            var hasher = new PasswordHasher<AppUser>();
            var passwordHash = hasher.HashPassword(null, "Admin@123");

            var tenantAdminRole = await _roleRepo.GetByNameAsync("TenantAdmin", cancellationToken);
            if (tenantAdminRole == null)
            {
                _logger.LogWarning("⚠️ TenantAdmin role not found. Skipping for {TenantName}.", tenant.Name);
                return;
            }

            var tenantAdmin = new AppUser(
                tenant.Id,
                $"{tenant.Name} Admin",
                $"{tenant.City?.ToLower() ?? "tenant"}_{Guid.NewGuid():N}@kaappaan.com",
                "0000000000",
                passwordHash,
                "TenantAdmin"
            );

            tenantAdmin.UserRoles.Add(new UserRole(tenantAdmin.Id, tenantAdminRole.Id));
            tenantAdmin.RequirePasswordChange(); // 👈 force change at first login
            await _dbContext.AddEntityAsync(tenantAdmin, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("✅ TenantAdmin created for {TenantName}", tenant.Name);
        }
    }
}
