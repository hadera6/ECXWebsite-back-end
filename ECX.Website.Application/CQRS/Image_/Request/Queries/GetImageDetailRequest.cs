using ECX.Website.Application.DTOs.Image;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Image_.Request.Queries
{
    public class GetImageDetailRequest :IRequest<BaseCommonResponse>
    {
        public string Id { get; set; }
    }
}
