using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.TrainingDoc_.Request.Command;
using ECX.Website.Application.DTOs.TrainingDoc;
using ECX.Website.Application.DTOs.TrainingDoc.Validators;
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

namespace ECX.Website.Application.CQRS.TrainingDoc_.Handler.Command
{
    public class CreateTrainingDocCommandHandler : IRequestHandler<CreateTrainingDocCommand, BaseCommonResponse>
    {
        private ITrainingDocRepository _trainingDocRepository;
        private IMapper _mapper;
        
        public CreateTrainingDocCommandHandler(ITrainingDocRepository trainingDocRepository, IMapper mapper)
        {
            _trainingDocRepository = trainingDocRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateTrainingDocCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new TrainingDocCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.TrainingDocFormDto);

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
                    var pdfValidator = new PdfValidator();
                    var pdfValidationResult = await pdfValidator.ValidateAsync(request.TrainingDocFormDto.File);

                    if (pdfValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = pdfValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.TrainingDocFormDto.File.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() +"."+ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\pdf", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.TrainingDocFormDto.File.CopyTo(stream);
                        }
                        var TrainingDocDto = _mapper.Map<TrainingDocDto>(request.TrainingDocFormDto);
                        TrainingDocDto.FileName = fileName;

                        string trainingDocId ;
                        bool flag = true;

                        while (true)
                        {
                            trainingDocId = Guid.NewGuid().ToString();
                            flag = await _trainingDocRepository.Exists(trainingDocId);
                            if (flag == false)
                            {
                                TrainingDocDto.Id = trainingDocId;
                                break;
                            }
                        }

                        var data =_mapper.Map<TrainingDoc>(TrainingDocDto);
                        
                        var saveData = await _trainingDocRepository.Add(data);

                        response.Data = _mapper.Map<TrainingDocDto>(saveData);
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
