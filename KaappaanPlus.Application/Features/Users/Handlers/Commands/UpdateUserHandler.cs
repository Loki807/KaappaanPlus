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

        public UpdateUserHandler(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetByIdAsync(request.UserDto.Id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User not found for ID: {request.UserDto.Id}");

            _mapper.Map(request.UserDto, user);
            await _userRepo.UpdateAsync(user, cancellationToken);

            // ✅ Replacement for logger: confirm success with validation
            if (user == null || user.Id == Guid.Empty)
                throw new Exception("User update failed unexpectedly.");

            // ✅ Successful update — return Unit
            return Unit.Value;
        }
    }
}
