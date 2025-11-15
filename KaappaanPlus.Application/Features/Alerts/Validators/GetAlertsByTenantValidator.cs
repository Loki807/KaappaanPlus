using FluentValidation;
using KaappaanPlus.Application.Features.Alerts.Requests.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Validators
{
    public class GetAlertsByTenantValidator : AbstractValidator<GetAlertsByTenantIdQuery>
    {
        public GetAlertsByTenantValidator()
        {
            RuleFor(x => x.TenantId)
                .NotEmpty().WithMessage("TenantId is required.");
        }
    }
}
