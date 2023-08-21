using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.ExternalLink_.Request.Command;
using ECX.Website.Application.DTOs.ExternalLink;
using ECX.Website.Application.DTOs.ExternalLink.Validators;
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

namespace ECX.Website.Application.CQRS.ExternalLink_.Handler.Command
{
    public class CreateExternalLinkCommandHandler : IRequestHandler<CreateExternalLinkCommand, BaseCommonResponse>
    {
        private IExternalLinkRepository _externalLinkRepository;
        private IMapper _mapper;

        public CreateExternalLinkCommandHandler(IExternalLinkRepository externalLinkRepository, IMapper mapper)
        {
            _externalLinkRepository = externalLinkRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateExternalLinkCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new ExternalLinkCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ExternalLinkFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.ExternalLinkFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.ExternalLinkFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.ExternalLinkFormDto.ImgFile.CopyTo(stream);
                        }
                        var ExternalLinkDto = _mapper.Map<ExternalLinkDto>(request.ExternalLinkFormDto);
                        ExternalLinkDto.ImgName = fileName;

                        string externalLinkId;
                        bool flag = true;

                        while (true)
                        {
                            externalLinkId = Guid.NewGuid().ToString();
                            flag = await _externalLinkRepository.Exists(externalLinkId);
                            if (flag == false)
                            {
                                ExternalLinkDto.Id = externalLinkId;
                                break;
                            }
                        }

                        var data = _mapper.Map<ExternalLink>(ExternalLinkDto);

                        var saveData = await _externalLinkRepository.Add(data);

                        response.Data = _mapper.Map<ExternalLinkDto>(saveData);
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
