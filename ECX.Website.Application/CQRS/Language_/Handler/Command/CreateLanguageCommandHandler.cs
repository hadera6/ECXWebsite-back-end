﻿using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Language_.Request.Command;
using ECX.Website.Application.DTOs.Language;
using ECX.Website.Application.DTOs.Language.Validators;
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

namespace ECX.Website.Application.CQRS.Language_.Handler.Command
{
    public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, BaseCommonResponse>
    {
        private ILanguageRepository _languageRepository;
        private IMapper _mapper;

        public CreateLanguageCommandHandler(ILanguageRepository languageRepository, IMapper mapper)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new LanguageCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.LanguageFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.LanguageFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.LanguageFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.LanguageFormDto.ImgFile.CopyTo(stream);
                        }
                        var LanguageDto = _mapper.Map<LanguageDto>(request.LanguageFormDto);
                        LanguageDto.ImgName = fileName;

                        string languageId;
                        bool flag = true;

                        while (true)
                        {
                            languageId = Guid.NewGuid().ToString();
                            flag = await _languageRepository.Exists(languageId);
                            if (flag == false)
                            {
                                LanguageDto.Id = languageId;
                                break;
                            }
                        }

                        var data = _mapper.Map<Language>(LanguageDto);

                        var saveData = await _languageRepository.Add(data);

                        response.Data = _mapper.Map<LanguageDto>(saveData);
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