using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Message_.Request.Command;
using ECX.Website.Application.DTOs.Message;
using ECX.Website.Application.DTOs.Message.Validators;
using ECX.Website.Application.Exceptions;

using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using ECX.Website.Application.CQRS.Message_.Request.Queries;

namespace ECX.Website.Application.CQRS.Message_.Handler.Queries
{
    public class GetMessageListRequestHandler : IRequestHandler<GetMessageListRequest, BaseCommonResponse>
    {
        private IMessageRepository _messageRepository;
        private IMapper _mapper;
        public GetMessageListRequestHandler(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetMessageListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _messageRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<MessageDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
