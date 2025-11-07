using AutoMapper;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Citizens.DTOs;
using KaappaanPlus.Application.Features.Citizens.Requests.Quries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.Handlers.Queries
{
    public class GetCitizenByIdQueryHandler : IRequestHandler<GetCitizenByIdQuery, CitizenDto>
    {
        private readonly ICitizenRepository _citizenRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCitizenByIdQueryHandler> _logger;

        public GetCitizenByIdQueryHandler(ICitizenRepository citizenRepo, IMapper mapper, ILogger<GetCitizenByIdQueryHandler> logger)
        {
            _citizenRepo = citizenRepo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CitizenDto> Handle(GetCitizenByIdQuery request, CancellationToken cancellationToken)
        {
            var citizen = await _citizenRepo.GetByIdAsync(request.Id);
            if (citizen == null)
            {
                _logger.LogWarning($"Citizen not found: {request.Id}");
                throw new KeyNotFoundException("Citizen not found.");
            }

            return _mapper.Map<CitizenDto>(citizen);
        }
    }
}
