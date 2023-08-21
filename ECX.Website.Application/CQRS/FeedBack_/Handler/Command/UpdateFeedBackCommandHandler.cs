using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.FeedBack_.Request.Command;
using ECX.Website.Application.DTOs.FeedBack;
using ECX.Website.Application.DTOs.FeedBack.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.FeedBack_.Handler.Command
{
    public class UpdateFeedBackCommandHandler : IRequestHandler<UpdateFeedBackCommand, BaseCommonResponse>
    {
        private IFeedBackRepository _feedBackRepository;
        private IMapper _mapper;
        public UpdateFeedBackCommandHandler(IFeedBackRepository feedBackRepository, IMapper mapper)
        {
            _feedBackRepository = feedBackRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateFeedBackCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new FeedBackUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.FeedBackFormDto);
            var FeedBackDto = _mapper.Map<FeedBackDto>(request.FeedBackFormDto);
            var flag = await _feedBackRepository.Exists(request.FeedBackFormDto.Id);

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
                            nameof(FeedBack), request.FeedBackFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.FeedBackFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.FeedBackFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _feedBackRepository.GetById(
                                request.FeedBackFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.FeedBackFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.FeedBackFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            FeedBackDto.ImgName = fileName;
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
                    FeedBackDto.ImgName = (await _feedBackRepository.GetById(
                                request.FeedBackFormDto.Id)).ImgName;
                } 

                var updateData = await _feedBackRepository.GetById(request.FeedBackFormDto.Id);
                
                _mapper.Map(FeedBackDto, updateData);

                var data = await _feedBackRepository.Update(updateData);

                response.Data = _mapper.Map<FeedBackDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

