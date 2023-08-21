using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Blog_.Request.Command;
using ECX.Website.Application.DTOs.Blog;
using ECX.Website.Application.DTOs.Blog.Validators;
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

namespace ECX.Website.Application.CQRS.Blog_.Handler.Command
{
    public class CreateBlogCommandHandler : IRequestHandler<CreateBlogCommand, BaseCommonResponse>
    {
        private IBlogRepository _blogRepository;
        private IMapper _mapper;

        public CreateBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new BlogCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.BlogFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.BlogFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.BlogFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.BlogFormDto.ImgFile.CopyTo(stream);
                        }
                        var BlogDto = _mapper.Map<BlogDto>(request.BlogFormDto);
                        BlogDto.ImgName = fileName;

                        string blogId;
                        bool flag = true;

                        while (true)
                        {
                            blogId = Guid.NewGuid().ToString();
                            flag = await _blogRepository.Exists(blogId);
                            if (flag == false)
                            {
                                BlogDto.Id = blogId;
                                break;
                            }
                        }

                        var data = _mapper.Map<Blog>(BlogDto);

                        var saveData = await _blogRepository.Add(data);

                        response.Data = _mapper.Map<BlogDto>(saveData);
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
