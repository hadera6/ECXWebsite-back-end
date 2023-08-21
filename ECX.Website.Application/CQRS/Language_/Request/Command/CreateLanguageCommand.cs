using ECX.Website.Application.DTOs.Language;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Language_.Request.Command
{
    public class CreateLanguageCommand : IRequest<BaseCommonResponse>
    {
        public LanguageFormDto LanguageFormDto { get; set; }
    }
}
