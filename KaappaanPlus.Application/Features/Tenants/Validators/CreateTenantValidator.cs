using FluentValidation;
using KaappaanPlus.Application.Features.Tenants.DTOs;
using KaappaanPlus.Application.Features.Tenants.Requests.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Tenants.Validators
{
    public class CreateTenantValidator : AbstractValidator<CreateTenantCommand>
    {
        public CreateTenantValidator()
        {
            RuleFor(x => x.TenantDto.Name)
                .NotEmpty().WithMessage("Tenant name is required.");

            RuleFor(x => x.TenantDto.Code)
                .NotEmpty().WithMessage("Tenant code is required.");

            RuleFor(x => x.TenantDto.City)
                .NotEmpty().WithMessage("City is required.");

            RuleFor(x => x.TenantDto.ServiceType)
                .NotEmpty().WithMessage("ServiceType is required.")
                .Must(s => new[] { "Police", "Fire", "Ambulance" }.Contains(s))
                .WithMessage("ServiceType must be one of: Police, Fire, or Ambulance.");
        }
    }
}
