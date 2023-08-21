using ECX.Website.Application.DTOs.FeedBack;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.FeedBack_.Request.Command
{
    public class UpdateFeedBackCommand :IRequest<BaseCommonResponse>
    {
        public FeedBackFormDto FeedBackFormDto { get; set; }

    }
}
