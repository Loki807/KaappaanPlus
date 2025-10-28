using FluentValidation;
using KaappaanPlus.Application.Features.Auth.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Auth.Validators
{
    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.OldPassword).NotEmpty();
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long");
        }
    }

}
