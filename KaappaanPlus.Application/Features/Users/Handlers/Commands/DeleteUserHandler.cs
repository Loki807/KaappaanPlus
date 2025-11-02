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
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<DeleteUserHandler> _logger;

        public DeleteUserHandler(IUserRepository userRepo, ILogger<DeleteUserHandler> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _userRepo.DeleteAsync(request.Id, cancellationToken);
            _logger.LogInformation("🗑️ User {UserId} deleted successfully.", request.Id);
            return Unit.Value;
        }
    }
}
