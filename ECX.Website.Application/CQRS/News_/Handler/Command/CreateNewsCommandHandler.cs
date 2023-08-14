using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.News_.Request.Command;
using ECX.Website.Application.DTOs.News;
using ECX.Website.Application.DTOs.News.Validators;
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

namespace ECX.Website.Application.CQRS.News_.Handler.Command
{
    public class CreateNewsCommandHandler : IRequestHandler<CreateNewsCommand, BaseCommonResponse>
    {
        private INewsRepository _newsRepository;
        private IMapper _mapper;
        
        public CreateNewsCommandHandler(INewsRepository newsRepository, IMapper mapper)
        {
            _newsRepository = newsRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateNewsCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new NewsCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.NewsFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.NewsFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.NewsFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() +"."+ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.NewsFormDto.ImgFile.CopyTo(stream);
                        }
                        var NewsDto = _mapper.Map<NewsDto>(request.NewsFormDto);
                        NewsDto.ImgName = fileName;

                        string newsId ;
                        bool flag = true;

                        while (true)
                        {
                            newsId = (Guid.NewGuid()).ToString();
                            flag = await _newsRepository.Exists(newsId);
                            if (flag == false)
                            {
                                NewsDto.Id = newsId;
                                break;
                            }
                        }

                        var data =_mapper.Map<News>(NewsDto);
                        
                        var saveData = await _newsRepository.Add(data);

                        response.Data = _mapper.Map<NewsDto>(saveData);
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
