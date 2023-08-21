using ECX.Website.Application.DTOs.Faq;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Faq_.Request.Queries
{
    public class GetFaqDetailRequest :IRequest<BaseCommonResponse>
    {
        public string Id { get; set; }
    }
}
