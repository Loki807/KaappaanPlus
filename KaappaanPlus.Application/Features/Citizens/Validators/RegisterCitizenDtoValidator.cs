using FluentValidation;
using KaappaanPlus.Application.Features.Citizens.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Citizens.Validators
{
    public class RegisterCitizenDtoValidator : AbstractValidator<CitizenRegisterDto>
    {
        public RegisterCitizenDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^(07\d{8})$")
                .WithMessage("Phone number must start with 07 and have 10 digits.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.NIC)
                .NotEmpty().WithMessage("NIC is required.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.");

            RuleFor(x => x.EmergencyContact)
                .NotEmpty().WithMessage("Emergency contact is required.")
                .Matches(@"^(07\d{8})$")
                .WithMessage("Emergency contact must start with 07 and have 10 digits.");
        }
    }
}
