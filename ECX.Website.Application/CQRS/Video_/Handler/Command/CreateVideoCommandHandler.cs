using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Video_.Request.Command;
using ECX.Website.Application.DTOs.Video;
using ECX.Website.Application.DTOs.Video.Validators;
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

namespace ECX.Website.Application.CQRS.Video_.Handler.Command
{
    public class CreateVideoCommandHandler : IRequestHandler<CreateVideoCommand, BaseCommonResponse>
    {
        private IVideoRepository _videoRepository;
        private IMapper _mapper;

        public CreateVideoCommandHandler(IVideoRepository videoRepository, IMapper mapper)
        {
            _videoRepository = videoRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new VideoCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.VideoFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.VideoFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.VideoFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.VideoFormDto.ImgFile.CopyTo(stream);
                        }
                        var VideoDto = _mapper.Map<VideoDto>(request.VideoFormDto);
                        VideoDto.ImgName = fileName;

                        string videoId;
                        bool flag = true;

                        while (true)
                        {
                            videoId = Guid.NewGuid().ToString();
                            flag = await _videoRepository.Exists(videoId);
                            if (flag == false)
                            {
                                VideoDto.Id = videoId;
                                break;
                            }
                        }

                        var data = _mapper.Map<Video>(VideoDto);

                        var saveData = await _videoRepository.Add(data);

                        response.Data = _mapper.Map<VideoDto>(saveData);
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
