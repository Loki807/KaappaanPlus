using AutoMapper;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Users.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Handlers.Commands
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepo;
        private readonly ITenantRepository _tenantRepo;
        private readonly IMapper _mapper;

        public CreateUserHandler(
            IUserRepository userRepo,
            ITenantRepository tenantRepo,
            IMapper mapper)
        {
            _userRepo = userRepo;
            _tenantRepo = tenantRepo;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Map the DTO to the User entity
            var user = _mapper.Map<AppUser>(request.UserDto);

            // Check if the TenantId is null and throw an exception or handle accordingly
            if (user.TenantId == null)
            {
                throw new ArgumentException("TenantId cannot be null");
            }

            // Fetch the tenant based on the TenantId in the User
            var tenant = await _tenantRepo.GetByIdAsync(user.TenantId.Value, cancellationToken);
            if (tenant == null)
                throw new KeyNotFoundException($"Tenant not found for ID: {user.TenantId}");

            // Assign the user role based on the tenant's ServiceType (Case-Insensitive)
            // Normalize to Title Case or use explicit casing in switch
            string type = tenant.ServiceType?.Trim() ?? "";

            user.Role = type switch
            {
                var t when t.Equals("Police", StringComparison.OrdinalIgnoreCase) => "Police",
                var t when t.Equals("Fire", StringComparison.OrdinalIgnoreCase) => "Fire",
                var t when t.Equals("Ambulance", StringComparison.OrdinalIgnoreCase) => "Ambulance",
                var t when t.Equals("University", StringComparison.OrdinalIgnoreCase) => "UniversityStaff",
                _ => "Citizen"
            };

            // Check if the user already exists based on email
            var existing = await _userRepo.GetByEmailAsync(user.Email, cancellationToken);
            if (existing != null)
                throw new InvalidOperationException($"User with email {user.Email} already exists.");

            // Hash the password
            var hasher = new PasswordHasher<AppUser>();
            var hashedPassword = hasher.HashPassword(user, request.UserDto.Password);
            user.SetPasswordHash(hashedPassword);

            // Create the user in the repository
            await _userRepo.CreateUserAsync(user, cancellationToken);

            // Return the user's Id
            return user.Id;
        }

    }
}
