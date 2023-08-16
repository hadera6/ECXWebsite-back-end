using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.PageCatagory.Validators
{
    public class PageCatagoryDtoValidator : AbstractValidator<PageCatagoryDto>
    {
        public PageCatagoryDtoValidator()
        {
            RuleFor(p=>p.Title)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
            RuleFor(p => p.Description)
               .NotEmpty().WithMessage("{PropertyName} is requiered.")
               .NotNull();
        }
    }
}
