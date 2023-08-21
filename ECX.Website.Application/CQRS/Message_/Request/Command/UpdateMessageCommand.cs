using ECX.Website.Application.DTOs.Message;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Message_.Request.Command
{
    public class UpdateMessageCommand :IRequest<BaseCommonResponse>
    {
        public MessageFormDto MessageFormDto { get; set; }

    }
}
