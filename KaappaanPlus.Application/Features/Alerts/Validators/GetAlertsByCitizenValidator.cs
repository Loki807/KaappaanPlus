using FluentValidation;
using KaappaanPlus.Application.Features.Alerts.Requests.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaappaanPlus.Application.Features.Alerts.Validators
{
    public class GetAlertsByCitizenValidator : AbstractValidator<GetAlertsByCitizenQuery>
    {
        public GetAlertsByCitizenValidator()
        {
            RuleFor(x => x.CitizenId)
                .NotEmpty().WithMessage("CitizenId is required.");
        }
    }
}
