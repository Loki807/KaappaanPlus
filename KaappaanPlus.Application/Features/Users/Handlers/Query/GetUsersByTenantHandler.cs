using AutoMapper;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Users.DTOs;
using KaappaanPlus.Application.Features.Users.Requests.Query;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Handlers.Query
{
    public class GetUsersByTenantHandler : IRequestHandler<GetUsersByTenantQuery, List<UserResponseDto>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public GetUsersByTenantHandler(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<List<UserResponseDto>> Handle(GetUsersByTenantQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepo.GetByTenantIdAsync(request.TenantId, cancellationToken);
            return _mapper.Map<List<UserResponseDto>>(users);
        }
    }
}
