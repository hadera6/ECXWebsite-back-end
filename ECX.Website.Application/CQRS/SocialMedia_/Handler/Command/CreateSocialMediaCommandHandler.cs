using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.SocialMedia_.Request.Command;
using ECX.Website.Application.DTOs.SocialMedia;
using ECX.Website.Application.DTOs.SocialMedia.Validators;
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

namespace ECX.Website.Application.CQRS.SocialMedia_.Handler.Command
{
    public class CreateSocialMediaCommandHandler : IRequestHandler<CreateSocialMediaCommand, BaseCommonResponse>
    {
        private ISocialMediaRepository _socialMediaRepository;
        private IMapper _mapper;

        public CreateSocialMediaCommandHandler(ISocialMediaRepository socialMediaRepository, IMapper mapper)
        {
            _socialMediaRepository = socialMediaRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateSocialMediaCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new SocialMediaCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.SocialMediaFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.SocialMediaFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.SocialMediaFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.SocialMediaFormDto.ImgFile.CopyTo(stream);
                        }
                        var SocialMediaDto = _mapper.Map<SocialMediaDto>(request.SocialMediaFormDto);
                        SocialMediaDto.ImgName = fileName;

                        string socialMediaId;
                        bool flag = true;

                        while (true)
                        {
                            socialMediaId = Guid.NewGuid().ToString();
                            flag = await _socialMediaRepository.Exists(socialMediaId);
                            if (flag == false)
                            {
                                SocialMediaDto.Id = socialMediaId;
                                break;
                            }
                        }

                        var data = _mapper.Map<SocialMedia>(SocialMediaDto);

                        var saveData = await _socialMediaRepository.Add(data);

                        response.Data = _mapper.Map<SocialMediaDto>(saveData);
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
