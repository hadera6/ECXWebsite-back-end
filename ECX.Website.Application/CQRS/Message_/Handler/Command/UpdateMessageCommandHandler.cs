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
                if (request.MessageFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.MessageFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _messageRepository.GetById(
                                request.MessageFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.MessageFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.MessageFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            MessageDto.ImgName = fileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Success = false;
                        response.Message = "Update Failed";
                        response.Errors = new List<string> { ex.Message };
                        response.Status = "400";
                    }
                }
                else
                {
                    MessageDto.ImgName = (await _messageRepository.GetById(
                                request.MessageFormDto.Id)).ImgName;
                } 

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

