using AutoMapper;
using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Users.Requests.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Handlers.Commands
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Unit>
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
            var dto = request.UpdateUserDto;

            // 🔎 1. Fetch user from DB
            var user = await _userRepo.GetByIdAsync(dto.Id, cancellationToken);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {dto.Id} not found.");

            // ✏️ 2. Update fields
            user.UpdateInfo(dto.Name, dto.Phone, dto.Role, dto.IsActive);

            // 💾 3. Save
            await _userRepo.UpdateAsync(user, cancellationToken);

            return Unit.Value;
        }
    }
}
