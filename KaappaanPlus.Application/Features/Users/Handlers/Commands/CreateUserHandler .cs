using AutoMapper;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Users.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Handlers.Commands
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepo;
        private readonly ITenantRepository _tenantRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateUserHandler> _logger;

        public CreateUserHandler(
            IUserRepository userRepo,
            ITenantRepository tenantRepo,
            IMapper mapper,
            ILogger<CreateUserHandler> logger)
        {
            _userRepo = userRepo;
            _tenantRepo = tenantRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<AppUser>(request.UserDto);

            if (!await _tenantRepo.ExistsAsync(user.TenantId, cancellationToken))
                throw new KeyNotFoundException($"Tenant not found for ID: {user.TenantId}");

            var existing = await _userRepo.GetByEmailAsync(user.Email, cancellationToken);
            if (existing != null)
                throw new InvalidOperationException($"User with email {user.Email} already exists.");

            var hasher = new PasswordHasher<AppUser>();
            var hashedPassword = hasher.HashPassword(user, request.UserDto.Password);
            user.SetPasswordHash(hashedPassword);

            await _userRepo.CreateUserAsync(user, cancellationToken);
            _logger.LogInformation("✅ User {Email} created for Tenant {TenantId}", user.Email, user.TenantId);

            return user.Id;
        }
    }
}
