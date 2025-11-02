using AutoMapper;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Users.DTOs;
using KaappaanPlus.Application.Features.Users.Requests.Query;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Handlers.Query
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllUsersHandler> _logger;

        public GetAllUsersHandler(IUserRepository userRepo, IMapper mapper, ILogger<GetAllUsersHandler> logger)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepo.GetByTenantIdAsync(request.TenantId, cancellationToken);
            _logger.LogInformation("📄 Retrieved {Count} users for Tenant {TenantId}", users.Count(), request.TenantId);
            return _mapper.Map<List<UserDto>>(users);
        }
    }
}
