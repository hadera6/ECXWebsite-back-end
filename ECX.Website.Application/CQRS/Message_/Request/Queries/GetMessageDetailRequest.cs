using ECX.Website.Application.DTOs.Message;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Message_.Request.Queries
{
    public class GetMessageDetailRequest :IRequest<BaseCommonResponse>
    {
        public string Id { get; set; }
    }
}
