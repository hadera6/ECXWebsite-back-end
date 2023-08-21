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

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Faild";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else
            {
                try
                {
                    var imageValidator = new ImageValidator();
                    var imgValidationResult = await imageValidator.ValidateAsync(request.MessageFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.MessageFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.MessageFormDto.ImgFile.CopyTo(stream);
                        }
                        var MessageDto = _mapper.Map<MessageDto>(request.MessageFormDto);
                        MessageDto.ImgName = fileName;

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
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = "Creation Failed";
                    response.Errors = new List<string> { ex.Message };
                    response.Status = "400";
                }
            }
            return response;
        }
    }
}
