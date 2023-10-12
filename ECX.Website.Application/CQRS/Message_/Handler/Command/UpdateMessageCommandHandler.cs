using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Message_.Request.Command;
using ECX.Website.Application.DTOs.Message;
using ECX.Website.Application.DTOs.Message.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Message_.Handler.Command
{
    public class UpdateMessageCommandHandler : IRequestHandler<UpdateMessageCommand, BaseCommonResponse>
    {
        private IMessageRepository _messageRepository;
        private IMapper _mapper;
        public UpdateMessageCommandHandler(IMessageRepository messageRepository, IMapper mapper)
        {
            _messageRepository = messageRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateMessageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new MessageUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.MessageFormDto);
            var MessageDto = _mapper.Map<MessageDto>(request.MessageFormDto);
            var flag = await _messageRepository.Exists(request.MessageFormDto.Id);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Update Failed";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else if (flag == false)
            {

                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Message), request.MessageFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                var updateData = await _messageRepository.GetById(request.MessageFormDto.Id);
                
                _mapper.Map(MessageDto, updateData);

                var data = await _messageRepository.Update(updateData);

                response.Data = _mapper.Map<MessageDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
}

