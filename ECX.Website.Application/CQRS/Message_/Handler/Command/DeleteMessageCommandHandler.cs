using AutoMapper;
using ECX.Website.Application.CQRS.Message_.Request.Command;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Contracts.Persistence;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECX.Website.Domain;
using ECX.Website.Application.Response;

namespace ECX.Website.Application.CQRS.Message_.Handler.Command
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, BaseCommonResponse>
    {
        
        private IMessageRepository _messageRepository;
        private IMapper _mapper;
        public DeleteMessageCommandHandler(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var data = await _messageRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Message), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _messageRepository.Delete(data);

                response.Success = true;
                response.Message = "Successfully Deleted";
                response.Status = "200";

            }
                
            return response;
        }
    }
}
