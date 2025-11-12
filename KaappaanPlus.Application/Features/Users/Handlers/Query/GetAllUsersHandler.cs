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

        public GetAllUsersHandler(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepo.GetByTenantIdAsync(request.TenantId, cancellationToken);

            if (users == null || !users.Any())
                throw new Exception($"No users found for Tenant ID: {request.TenantId}");

            var mappedUsers = _mapper.Map<List<UserDto>>(users);

            if (mappedUsers == null || mappedUsers.Count == 0)
                throw new Exception("Failed to map user data.");

            // ✅ Success — return mapped list
            return mappedUsers;
        }
    }
}
