using FluentValidation;
using KaappaanPlus.Application.Features.Users.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Users.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            // 🆔 Must have valid ID
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required.");

            // 👤 Name validation
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100)
                .WithMessage("User name cannot be empty or exceed 100 characters.");

            // ☎️ Optional phone format check
            RuleFor(x => x.Phone)
                .Matches(@"^\d{10}$")
                .When(x => !string.IsNullOrWhiteSpace(x.Phone))
                .WithMessage("Phone must be a valid 10-digit number.");

            // 🏷️ Optional Role
            RuleFor(x => x.Role)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.Role))
                .WithMessage("Role name too long.");

            // ✅ Active status check (true/false always valid)
            RuleFor(x => x.IsActive)
                .NotNull()
                .WithMessage("Active status must be provided.");
        }
    }
}
