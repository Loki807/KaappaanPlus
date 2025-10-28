using AutoMapper;
using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Users.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
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
        private readonly Contracts.IUserRepository _userRepo;
        private readonly ITenantRepository _tenantRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateUserHandler> _logger;

        public CreateUserHandler(
            Contracts.IUserRepository userRepo,
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
            // ✅ 1) Map DTO → Entity
            var user = _mapper.Map<AppUser>(request.UserDto);

            // ✅ 2) Tenant validation (via Repository)
            var tenantExists = await _tenantRepo.ExistsAsync(user.TenantId, cancellationToken);
            if (!tenantExists)
                throw new KeyNotFoundException($"Tenant not found for ID: {user.TenantId}");

            // ✅ 3) Email duplication check
            var existing = await _userRepo.GetByEmailAsync(user.Email, cancellationToken);
            if (existing != null)
                throw new InvalidOperationException($"User with email {user.Email} already exists.");

            // ✅ 4) Create
            await _userRepo.CreateUserAsync(user, cancellationToken);
            _logger.LogInformation("✅ User {Email} created under Tenant {TenantId}", user.Email, user.TenantId);

            return user.Id;
        }
    }
}

