using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.PageCatagory_.Request.Command;
using ECX.Website.Application.DTOs.PageCatagory;
using ECX.Website.Application.DTOs.PageCatagory.Validators;
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

namespace ECX.Website.Application.CQRS.PageCatagory_.Handler.Command
{
    public class CreatePageCatagoryCommandHandler : IRequestHandler<CreatePageCatagoryCommand, BaseCommonResponse>
    {
        private IPageCatagoryRepository _pageCatagoryRepository;
        private IMapper _mapper;
        
        public CreatePageCatagoryCommandHandler(IPageCatagoryRepository pageCatagoryRepository, IMapper mapper)
        {
            _pageCatagoryRepository = pageCatagoryRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreatePageCatagoryCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new PageCatagoryCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.PageCatagoryFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.PageCatagoryFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.PageCatagoryFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() +"."+ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.PageCatagoryFormDto.ImgFile.CopyTo(stream);
                        }
                        var PageCatagoryDto = _mapper.Map<PageCatagoryDto>(request.PageCatagoryFormDto);
                        PageCatagoryDto.ImgName = fileName;

                        string pageCatagoryId ;
                        bool flag = true;

                        while (true)
                        {
                            pageCatagoryId = (Guid.NewGuid()).ToString();
                            flag = await _pageCatagoryRepository.Exists(pageCatagoryId);
                            if (flag == false)
                            {
                                PageCatagoryDto.Id = pageCatagoryId;
                                break;
                            }
                        }

                        var data =_mapper.Map<PageCatagory>(PageCatagoryDto);
                        
                        var saveData = await _pageCatagoryRepository.Add(data);

                        response.Data = _mapper.Map<PageCatagoryDto>(saveData);
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
