using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("Tenant ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress().WithMessage("A valid email is required.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^[0-9]{9,15}$").WithMessage("Phone number must be between 9 and 15 digits.");

            RuleFor(x => x.Password)
                .NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(BeAValidRole).WithMessage("Invalid role. Allowed: SuperAdmin, TenantAdmin, Citizen, Police, Fire, Traffic, Ambulance.");
        }

        private bool BeAValidRole(string role)
        {
            var validRoles = new[]
            {
                "SuperAdmin", "TenantAdmin", "Citizen", "Police", "Fire", "Traffic", "Ambulance"
            };
            return validRoles.Contains(role);
        }
    }
}
