using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.SocialMedia_.Request.Command;
using ECX.Website.Application.DTOs.SocialMedia;
using ECX.Website.Application.DTOs.SocialMedia.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.SocialMedia_.Handler.Command
{
    public class UpdateSocialMediaCommandHandler : IRequestHandler<UpdateSocialMediaCommand, BaseCommonResponse>
    {
        private ISocialMediaRepository _socialMediaRepository;
        private IMapper _mapper;
        public UpdateSocialMediaCommandHandler(ISocialMediaRepository socialMediaRepository, IMapper mapper)
        {
            _socialMediaRepository = socialMediaRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateSocialMediaCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new SocialMediaUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.SocialMediaFormDto);
            var SocialMediaDto = _mapper.Map<SocialMediaDto>(request.SocialMediaFormDto);
            var flag = await _socialMediaRepository.Exists(request.SocialMediaFormDto.Id);

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
                            nameof(SocialMedia), request.SocialMediaFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.SocialMediaFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.SocialMediaFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _socialMediaRepository.GetById(
                                request.SocialMediaFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.SocialMediaFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.SocialMediaFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            SocialMediaDto.ImgName = fileName;
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
                    SocialMediaDto.ImgName = (await _socialMediaRepository.GetById(
                                request.SocialMediaFormDto.Id)).ImgName;
                } 

                var updateData = await _socialMediaRepository.GetById(request.SocialMediaFormDto.Id);
                
                _mapper.Map(SocialMediaDto, updateData);

                var data = await _socialMediaRepository.Update(updateData);

                response.Data = _mapper.Map<SocialMediaDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

