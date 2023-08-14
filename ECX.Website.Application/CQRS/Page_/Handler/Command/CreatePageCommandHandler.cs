using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Page_.Request.Command;
using ECX.Website.Application.DTOs.Page;
using ECX.Website.Application.DTOs.Page.Validators;
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

namespace ECX.Website.Application.CQRS.Page_.Handler.Command
{
    public class CreatePageCommandHandler : IRequestHandler<CreatePageCommand, BaseCommonResponse>
    {
        private IPageRepository _pageRepository;
        private IMapper _mapper;
        
        public CreatePageCommandHandler(IPageRepository pageRepository, IMapper mapper)
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreatePageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new PageCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.PageFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.PageFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.PageFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() +"."+ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.PageFormDto.ImgFile.CopyTo(stream);
                        }
                        var PageDto = _mapper.Map<PageDto>(request.PageFormDto);
                        PageDto.ImgName = fileName;

                        string pageId ;
                        bool flag = true;

                        while (true)
                        {
                            pageId = (Guid.NewGuid()).ToString();
                            flag = await _pageRepository.Exists(pageId);
                            if (flag == false)
                            {
                                PageDto.Id = pageId;
                                break;
                            }
                        }

                        var data =_mapper.Map<Page>(PageDto);
                        
                        var saveData = await _pageRepository.Add(data);

                        response.Data = _mapper.Map<PageDto>(saveData);
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
