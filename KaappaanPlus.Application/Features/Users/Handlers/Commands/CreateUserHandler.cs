using AutoMapper;
using KaappaanPlus.Application.Contracts;
using KaappaanPlus.Application.Features.Users.Requests.Commands;
using KaappaanPlus.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Handlers.Commands
{

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IAppDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateUserHandler(IAppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<AppUser>(request.UserDto);

            var tenantExists = _dbContext.Tenants.Any(t => t.Id == user.TenantId);
            if (!tenantExists)
                throw new Exception($"Tenant not found for ID: {user.TenantId}");

            await _dbContext.AddEntityAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return user.Id;
        }


    }
}

