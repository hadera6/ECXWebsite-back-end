using ECX.Website.Application.DTOs.Publication;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Publication_.Request.Queries
{
    public class GetPublicationDetailRequest :IRequest<BaseCommonResponse>
    {
        public string Id { get; set; }
    }
}
