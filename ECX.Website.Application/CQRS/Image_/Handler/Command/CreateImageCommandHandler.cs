using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Image_.Request.Command;
using ECX.Website.Application.DTOs.Image;
using ECX.Website.Application.DTOs.Image.Validators;
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

namespace ECX.Website.Application.CQRS.Image_.Handler.Command
{
    public class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, BaseCommonResponse>
    {
        private IImageRepository _imageRepository;
        private IMapper _mapper;
        
        public CreateImageCommandHandler(IImageRepository imageRepository, IMapper mapper)
        {
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateImageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new ImageCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ImageFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.ImageFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.ImageFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() +"."+ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.ImageFormDto.ImgFile.CopyTo(stream);
                        }
                        var ImageDto = _mapper.Map<ImageDto>(request.ImageFormDto);
                        ImageDto.ImgName = fileName;

                        string imageId ;
                        bool flag = true;

                        while (true)
                        {
                            imageId = (Guid.NewGuid()).ToString();
                            flag = await _imageRepository.Exists(imageId);
                            if (flag == false)
                            {
                                ImageDto.Id = imageId;
                                break;
                            }
                        }

                        var data =_mapper.Map<Image>(ImageDto);
                        
                        var saveData = await _imageRepository.Add(data);

                        response.Data = _mapper.Map<ImageDto>(saveData);
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
