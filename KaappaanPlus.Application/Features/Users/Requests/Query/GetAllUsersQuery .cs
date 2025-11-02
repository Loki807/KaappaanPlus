using KaappaanPlus.Application.Features.Users.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Requests.Query
{
    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {
        public Guid TenantId { get; set; }
    }
}
