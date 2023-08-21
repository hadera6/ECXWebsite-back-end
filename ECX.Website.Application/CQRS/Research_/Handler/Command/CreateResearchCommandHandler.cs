using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Research_.Request.Command;
using ECX.Website.Application.DTOs.Research;
using ECX.Website.Application.DTOs.Research.Validators;
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

namespace ECX.Website.Application.CQRS.Research_.Handler.Command
{
    public class CreateResearchCommandHandler : IRequestHandler<CreateResearchCommand, BaseCommonResponse>
    {
        private IResearchRepository _researchRepository;
        private IMapper _mapper;

        public CreateResearchCommandHandler(IResearchRepository researchRepository, IMapper mapper)
        {
            _researchRepository = researchRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateResearchCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new ResearchCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ResearchFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.ResearchFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.ResearchFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.ResearchFormDto.ImgFile.CopyTo(stream);
                        }
                        var ResearchDto = _mapper.Map<ResearchDto>(request.ResearchFormDto);
                        ResearchDto.ImgName = fileName;

                        string researchId;
                        bool flag = true;

                        while (true)
                        {
                            researchId = Guid.NewGuid().ToString();
                            flag = await _researchRepository.Exists(researchId);
                            if (flag == false)
                            {
                                ResearchDto.Id = researchId;
                                break;
                            }
                        }

                        var data = _mapper.Map<Research>(ResearchDto);

                        var saveData = await _researchRepository.Add(data);

                        response.Data = _mapper.Map<ResearchDto>(saveData);
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
