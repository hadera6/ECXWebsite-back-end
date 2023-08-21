using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Training_.Request.Command;
using ECX.Website.Application.DTOs.Training;
using ECX.Website.Application.DTOs.Training.Validators;
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

namespace ECX.Website.Application.CQRS.Training_.Handler.Command
{
    public class CreateTrainingCommandHandler : IRequestHandler<CreateTrainingCommand, BaseCommonResponse>
    {
        private ITrainingRepository _trainingRepository;
        private IMapper _mapper;

        public CreateTrainingCommandHandler(ITrainingRepository trainingRepository, IMapper mapper)
        {
            _trainingRepository = trainingRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateTrainingCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new TrainingCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.TrainingFormDto);

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
                    var imgValidationResult = await imageValidator.ValidateAsync(request.TrainingFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.TrainingFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.TrainingFormDto.ImgFile.CopyTo(stream);
                        }
                        var TrainingDto = _mapper.Map<TrainingDto>(request.TrainingFormDto);
                        TrainingDto.ImgName = fileName;

                        string trainingId;
                        bool flag = true;

                        while (true)
                        {
                            trainingId = Guid.NewGuid().ToString();
                            flag = await _trainingRepository.Exists(trainingId);
                            if (flag == false)
                            {
                                TrainingDto.Id = trainingId;
                                break;
                            }
                        }

                        var data = _mapper.Map<Training>(TrainingDto);

                        var saveData = await _trainingRepository.Add(data);

                        response.Data = _mapper.Map<TrainingDto>(saveData);
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
