using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Citizens.Requests.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.Handlers.Commands
{
    public class DeleteCitizenHandler : IRequestHandler<DeleteCitizenCommand, Unit>
    {
        private readonly ICitizenRepository _citizenRepo;
        private readonly IUserRepository _userRepo;
       
        public DeleteCitizenHandler(ICitizenRepository citizenRepo, IUserRepository userRepo, ILogger<DeleteCitizenHandler> logger)
        {
            _citizenRepo = citizenRepo;
            _userRepo = userRepo;
          
        }

        public async Task<Unit> Handle(DeleteCitizenCommand request, CancellationToken cancellationToken)
        {
            var citizen = await _citizenRepo.GetByIdAsync(request.Id);
            if (citizen == null)
            {
                throw new KeyNotFoundException("Citizen not found.");
            }

            // ✅ Get linked AppUser
            var appUser = await _userRepo.GetByIdAsync(citizen.AppUserId);

            // ✅ Delete Citizen
            await _citizenRepo.DeleteAsync(citizen);

            // ✅ Optional: also delete AppUser
            if (appUser != null)
            {
                await _userRepo.DeleteAsync(appUser);

            }

          
            return Unit.Value;
        }
    }
}
