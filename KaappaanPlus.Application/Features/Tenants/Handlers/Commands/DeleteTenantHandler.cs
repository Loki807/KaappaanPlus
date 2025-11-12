using KaappaanPlus.Application.Common.Exceptions;
using KaappaanPlus.Application.Contracts.Persistence;
using KaappaanPlus.Application.Features.Tenants.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.Handlers.Commands
{
    public class DeleteTenantHandler : IRequestHandler<DeleteTenantCommand, Unit>
    {
        private readonly ITenantRepository _tenantRepo;
     
        public DeleteTenantHandler(ITenantRepository tenantRepo, ILogger<DeleteTenantHandler> logger)
        {
            _tenantRepo = tenantRepo;
          
        }

        public async Task<Unit> Handle(DeleteTenantCommand request, CancellationToken cancellationToken)
        {
            var tenant = await _tenantRepo.GetByIdAsync(request.Id, cancellationToken);

            if (tenant == null)
            {
              
                throw new NotFoundException(nameof(tenant), request.Id);
            }

            await _tenantRepo.DeleteAsync(request.Id, cancellationToken);
            
            return Unit.Value;
        }
    }
}
