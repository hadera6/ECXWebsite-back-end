using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.SocialMedia.Validators
{
    public class SocialMediaDtoValidator : AbstractValidator<SocialMediaDto>
    {
        public SocialMediaDtoValidator()
        {
            RuleFor(p => p.LangId)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
            RuleFor(p => p.Description)
               .NotEmpty().WithMessage("{PropertyName} is requiered.")
               .NotNull();
            RuleFor(p => p.Url)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
        }
    }
}
