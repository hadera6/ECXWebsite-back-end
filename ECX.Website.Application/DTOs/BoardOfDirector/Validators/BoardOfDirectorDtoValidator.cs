using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.BoardOfDirector.Validators
{
    public class BoardOfDirectorDtoValidator : AbstractValidator<BoardOfDirectorDto>
    {
        public BoardOfDirectorDtoValidator()
        {
            RuleFor(p=>p.Name)
                .NotEmpty().WithMessage("{PropertyName} is requiered.")
                .NotNull();
            RuleFor(p => p.Description)
               .NotEmpty().WithMessage("{PropertyName} is requiered.")
               .NotNull();
        }
    }
}
