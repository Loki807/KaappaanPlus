using AutoMapper;
using KaappaanPlus.Application.Common.Exceptions;
using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Tenants.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

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

        // ✅ Include Email directly from the request
        tenant.Email = request.TenantDto.Email;

        // ✅ Include ServiceType (if not mapped automatically)
        tenant.ServiceType = request.TenantDto.ServiceType ?? "General";

        // ✅ Generate Code before saving
        tenant.Code = $"{tenant.City?.Substring(0, 3).ToUpper() ?? "TEN"}_TENANT";

        // ✅ Check duplicate
        var existingTenant = await _tenantRepo.GetByNameOrCityAsync(tenant.Name, tenant.City!, cancellationToken);
        if (existingTenant != null)
            throw new ConflictException("Tenant", $"'{tenant.Name}' already exists.");

        // ✅ Save
        var tenantId = await _tenantRepo.AddAsync(tenant, cancellationToken);

        // ✅ Auto-create TenantAdmin with same email
        var alreadyHasAdmin = await _tenantRepo.TenantAdminExistsAsync(tenantId, cancellationToken);
        if (!alreadyHasAdmin)
            await CreateTenantAdminAsync(tenant, request.TenantDto.Email, cancellationToken);
        else
            return tenantId; // If already has TenantAdmin, skip creation

        return tenantId;
    }

    private async Task CreateTenantAdminAsync(Tenant tenant, string email, CancellationToken ct)
    {
        // 1️⃣ Get the TenantAdmin role
        var role = await _roleRepo.GetByNameAsync("TenantAdmin", ct);
        if (role == null)
        {
            throw new Exception($"Role 'TenantAdmin' not found for tenant {tenant.Name}");
        }

        // 3️⃣ Create the admin instance using the provided email
        var admin = new AppUser(
            tenant.Id,
            $"{tenant.Name} Admin",
            email,  // Use the provided email directly here
            "0000000000",  // Default phone number (can be replaced with a valid one)
            "",  // Other properties
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

        return;
    }
}

