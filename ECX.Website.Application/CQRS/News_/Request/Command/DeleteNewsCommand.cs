using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.News_.Request.Command
{
    public class DeleteNewsCommand : IRequest<BaseCommonResponse>
    {
        public string Id { get; set; }
    }
}
