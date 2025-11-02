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
        private readonly ILogger<GetUserByIdHandler> _logger;

        public GetUserByIdHandler(IUserRepository userRepo, IMapper mapper, ILogger<GetUserByIdHandler> logger)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.Id, cancellationToken);
            if (user == null) return null;

            _logger.LogInformation("📄 Retrieved user {UserId}", user.Id);
            return _mapper.Map<UserDto>(user);
        }
    }
}
