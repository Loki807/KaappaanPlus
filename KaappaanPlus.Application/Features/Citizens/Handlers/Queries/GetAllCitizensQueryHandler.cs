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
    public class GetAllCitizensQueryHandler : IRequestHandler<GetAllCitizensQuery, List<CitizenDto>>
    {
        private readonly ICitizenRepository _citizenRepo;
        private readonly IMapper _mapper;
       
        public GetAllCitizensQueryHandler(ICitizenRepository citizenRepo, IMapper mapper, ILogger<GetAllCitizensQueryHandler> logger)
        {
            _citizenRepo = citizenRepo;
            _mapper = mapper;
           
        }

        public async Task<List<CitizenDto>> Handle(GetAllCitizensQuery request, CancellationToken cancellationToken)
        {
            var citizens = await _citizenRepo.GetAllAsync();
            return _mapper.Map<List<CitizenDto>>(citizens);
        }
    }
}
