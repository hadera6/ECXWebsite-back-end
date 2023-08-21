using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.SocialMedia_.Request.Command
{
    public class DeleteSocialMediaCommand : IRequest<BaseCommonResponse>
    {
        public string Id { get; set; }
    }
}
