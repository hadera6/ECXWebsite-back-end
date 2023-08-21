using ECX.Website.Application.DTOs.ExternalLink;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.ExternalLink_.Request.Command
{
    public class CreateExternalLinkCommand : IRequest<BaseCommonResponse>
    {
        public ExternalLinkFormDto ExternalLinkFormDto { get; set; }
    }
}
