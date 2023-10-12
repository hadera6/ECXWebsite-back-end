using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.TrainingDoc_.Request.Command;
using ECX.Website.Application.DTOs.TrainingDoc;
using ECX.Website.Application.DTOs.TrainingDoc.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.TrainingDoc_.Handler.Command
{
    public class UpdateTrainingDocCommandHandler : IRequestHandler<UpdateTrainingDocCommand, BaseCommonResponse>
    {
        private ITrainingDocRepository _trainingDocRepository;
        private IMapper _mapper;
        public UpdateTrainingDocCommandHandler(ITrainingDocRepository trainingDocRepository, IMapper mapper)
        {
            _trainingDocRepository = trainingDocRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateTrainingDocCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new TrainingDocUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.TrainingDocFormDto);
            var TrainingDocDto = _mapper.Map<TrainingDocDto>(request.TrainingDocFormDto);
            var flag = await _trainingDocRepository.Exists(request.TrainingDocFormDto.Id);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Update Failed";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else if (flag == false)
            {

                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(TrainingDoc), request.TrainingDocFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.TrainingDocFormDto.File != null)
                {
                    try
                    {
                        var pdfValidator = new PdfValidator();
                        var pdfValidationResult = await pdfValidator.ValidateAsync(request.TrainingDocFormDto.File);

                        if (pdfValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = pdfValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldPdf = (await _trainingDocRepository.GetById(
                                request.TrainingDocFormDto.Id)).FileName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\pdf",oldPdf);
                            File.Delete(oldPath);

                            string contentType = request.TrainingDocFormDto.File.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\pdf", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.TrainingDocFormDto.File.CopyTo(stream);
                            }
                           
                            TrainingDocDto.FileName = fileName;
                        }
                    }
                    catch (Exception ex)
                    {
                        response.Success = false;
                        response.Message = "Update Failed";
                        response.Errors = new List<string> { ex.Message };
                        response.Status = "400";
                    }
                }
                else
                {
                    TrainingDocDto.FileName = (await _trainingDocRepository.GetById(
                                request.TrainingDocFormDto.Id)).FileName;
                } 

                var updateData = await _trainingDocRepository.GetById(request.TrainingDocFormDto.Id);
                
                _mapper.Map(TrainingDocDto, updateData);

                var data = await _trainingDocRepository.Update(updateData);

                response.Data = _mapper.Map<TrainingDocDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

