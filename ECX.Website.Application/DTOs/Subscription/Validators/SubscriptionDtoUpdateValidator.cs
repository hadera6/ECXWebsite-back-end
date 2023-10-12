using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Subscription.Validators
{
    public class SubscriptionUpdateDtoValidator : AbstractValidator<SubscriptionFormDto>
    {
        public SubscriptionUpdateDtoValidator()
        {

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
            RuleFor(p => p.SubscriberName)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
        }
    }
}
