using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Vacancy_.Request.Command;
using ECX.Website.Application.DTOs.Vacancy;
using ECX.Website.Application.DTOs.Vacancy.Validators;
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

namespace ECX.Website.Application.CQRS.Vacancy_.Handler.Command
{
    public class CreateVacancyCommandHandler : IRequestHandler<CreateVacancyCommand, BaseCommonResponse>
    {
        private IVacancyRepository _vacancyRepository;
        private IMapper _mapper;

        public CreateVacancyCommandHandler(IVacancyRepository vacancyRepository, IMapper mapper)
        {
            _vacancyRepository = vacancyRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateVacancyCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new VacancyCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.VacancyFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.VacancyFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.VacancyFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.VacancyFormDto.ImgFile.CopyTo(stream);
                        }
                        var VacancyDto = _mapper.Map<VacancyDto>(request.VacancyFormDto);
                        VacancyDto.ImgName = fileName;

                        string vacancyId;
                        bool flag = true;

                        while (true)
                        {
                            vacancyId = Guid.NewGuid().ToString();
                            flag = await _vacancyRepository.Exists(vacancyId);
                            if (flag == false)
                            {
                                VacancyDto.Id = vacancyId;
                                break;
                            }
                        }

                        var data = _mapper.Map<Vacancy>(VacancyDto);

                        var saveData = await _vacancyRepository.Add(data);

                        response.Data = _mapper.Map<VacancyDto>(saveData);
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
