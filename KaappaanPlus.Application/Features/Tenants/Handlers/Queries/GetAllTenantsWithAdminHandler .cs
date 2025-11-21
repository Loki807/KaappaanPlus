using AutoMapper;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Tenants.DTOs;
using KaappaanPlus.Application.Features.Tenants.Requests.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.Handlers.Queries
{
    public class GetAllTenantsWithAdminHandler
     : IRequestHandler<GetAllTenantsWithAdminQuery, List<TenantDto>>
    {
        private readonly ITenantRepository _tenantRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public GetAllTenantsWithAdminHandler(
            ITenantRepository tenantRepo,
            IUserRepository userRepo,
            IMapper mapper)
        {
            _tenantRepo = tenantRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<List<TenantDto>> Handle(GetAllTenantsWithAdminQuery request, CancellationToken ct)
        {
            var tenants = await _tenantRepo.GetAllAsync();

            var result = new List<TenantDto>();

            foreach (var tenant in tenants)
            {
                var admin = await _userRepo.GetTenantAdminByTenantIdAsync(tenant.Id);

                var dto = _mapper.Map<TenantDto>(tenant);

                dto.Email = admin?.Email ?? "N/A"; // add admin email

                result.Add(dto);
            }

            return result;
        }
    }


}
