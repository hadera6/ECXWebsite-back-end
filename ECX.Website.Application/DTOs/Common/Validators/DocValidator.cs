using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Common.Validators
{
    public class DocValidator : AbstractValidator<IFormFile>
    {
        public DocValidator()
        {
            RuleFor(x => x.ContentType)
                .NotNull()
                .Must(x => x.Equals("application/msword") || 
                        x.Equals("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
                .WithMessage("File must be Doc or Docx");
        }
    }
}
