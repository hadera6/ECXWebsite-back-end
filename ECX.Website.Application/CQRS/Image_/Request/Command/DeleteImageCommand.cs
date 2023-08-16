using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Image_.Request.Command
{
    public class DeleteImageCommand : IRequest<BaseCommonResponse>
    {
        public string Id { get; set; }
    }
}
