using AutoMapper;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Users.Requests.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Handlers.Commands
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateUserHandler> _logger;

        public UpdateUserHandler(IUserRepository userRepo, IMapper mapper, ILogger<UpdateUserHandler> logger)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.UserDto.Id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User not found for ID: {request.UserDto.Id}");

            _mapper.Map(request.UserDto, user);
            await _userRepo.UpdateAsync(user, cancellationToken);

            _logger.LogInformation("✅ User {UserId} updated successfully.", user.Id);
            return Unit.Value;
        }
    }
}
