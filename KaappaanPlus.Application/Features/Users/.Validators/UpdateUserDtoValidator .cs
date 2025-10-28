using FluentValidation;
using KaappaanPlus.Application.Features.Users.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Validators
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            // 🆔 Must have a valid Id
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required.");

            // 👤 Name validation
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            // 📞 Phone validation
            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^[0-9]{9,15}$").WithMessage("Phone number must be between 9 and 15 digits.");

            // 🧑‍💻 Role validation
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(BeAValidRole).WithMessage("Invalid role. Allowed: SuperAdmin, TenantAdmin, Citizen, Police, Fire, Traffic, Ambulance.");

            // ✅ IsActive flag is optional but required to be boolean
            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive flag must be set (true/false).");
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
