using FluentValidation;
using KaappaanPlus.Application.Features.Alerts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Validators
{
    public class CreateAlertValidator : AbstractValidator<CreateAlertDto>
    {
        public CreateAlertValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty();
            RuleFor(x => x.CreatedByUserId).NotEmpty();
            RuleFor(x => x.Type).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        }
    }
}
