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
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserResponseDto>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.Id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.Id} not found.");

            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
