using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Announcement.Validators
{
    public class AnnouncementCreateDtoValidator : AbstractValidator<AnnouncementFormDto>
    {
        public AnnouncementCreateDtoValidator()
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
            RuleFor(p => p.File)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
        }
    }
}
