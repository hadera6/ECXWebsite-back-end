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
using System.IO;
using ECX.Website.Application.DTOs.Common.Validators;

namespace ECX.Website.Application.CQRS.Message_.Handler.Command
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, BaseCommonResponse>
    {
        private IMessageRepository _messageRepository;
        private IMapper _mapper;

        public CreateMessageCommandHandler(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new MessageCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.MessageFormDto);
            var MessageDto = _mapper.Map<MessageDto>(request.MessageFormDto);
            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Faild";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else
            {
                string messageId;
                bool flag = true;

                while (true)
                {
                    messageId = Guid.NewGuid().ToString();
                    flag = await _messageRepository.Exists(messageId);
                    if (flag == false)
                    {
                        MessageDto.Id = messageId;
                        break;
                    }
                }

                var data = _mapper.Map<Message>(MessageDto);

                var saveData = await _messageRepository.Add(data);

                response.Data = _mapper.Map<MessageDto>(saveData);
                response.Success = true;
                response.Message = "Created Successfully";
                response.Status = "200";
                    
            }
            return response;
        }
    }
}