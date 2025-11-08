using AutoMapper;
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
    public class UpdateCitizenHandler : IRequestHandler<UpdateCitizenCommand, Unit>
    {
        private readonly ICitizenRepository _citizenRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<UpdateCitizenHandler> _logger;

        public UpdateCitizenHandler(ICitizenRepository citizenRepo, IUserRepository userRepo, ILogger<UpdateCitizenHandler> logger)
        {
            _citizenRepo = citizenRepo;
            _userRepo = userRepo;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateCitizenCommand request, CancellationToken cancellationToken)
        {
            var dto = request.CitizenDto;

            var citizen = await _citizenRepo.GetByIdAsync(dto.Id);
            if (citizen == null)
                throw new KeyNotFoundException("Citizen not found");

            var appUser = await _userRepo.GetByIdAsync(citizen.AppUserId);
            if (appUser == null)
                throw new KeyNotFoundException("Linked AppUser not found");

            // ✅ Update AppUser name
            appUser.Name = dto.FullName;

            // ✅ Update Citizen details
            citizen.UpdateProfile(dto.NIC, dto.Address, dto.EmergencyContact);

            // ✅ Save both
            await _userRepo.UpdateAsync(appUser);
            await _citizenRepo.UpdateAsync(citizen);

            _logger.LogInformation($"Citizen ({citizen.Id}) and AppUser ({appUser.Id}) updated successfully.");
            return Unit.Value;
        }
    }
}
