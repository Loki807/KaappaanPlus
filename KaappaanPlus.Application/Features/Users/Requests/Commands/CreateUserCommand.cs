using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Requests.Commands
{
    public class CreateUserCommand : IRequest<Guid> // returns new UserId
    {
        public CreateUserDto UserDto { get; set; } = default!;
    }
}
