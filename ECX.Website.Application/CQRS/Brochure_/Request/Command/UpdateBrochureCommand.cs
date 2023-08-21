using ECX.Website.Application.DTOs.Brochure;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Brochure_.Request.Command
{
    public class UpdateBrochureCommand :IRequest<BaseCommonResponse>
    {
        public BrochureFormDto BrochureFormDto { get; set; }

    }
}
