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
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
                throw new Exception($"User not found for ID: {request.Id}");

            var mappedUser = _mapper.Map<UserDto>(user);

            if (mappedUser == null)
                throw new Exception("Failed to map user data.");

            // ✅ Successfully retrieved user
            return mappedUser;
        }
    }
}
