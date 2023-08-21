using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Training_.Request.Command;
using ECX.Website.Application.DTOs.Training;
using ECX.Website.Application.DTOs.Training.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Training_.Handler.Command
{
    public class UpdateTrainingCommandHandler : IRequestHandler<UpdateTrainingCommand, BaseCommonResponse>
    {
        private ITrainingRepository _trainingRepository;
        private IMapper _mapper;
        public UpdateTrainingCommandHandler(ITrainingRepository trainingRepository, IMapper mapper)
        {
            _trainingRepository = trainingRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateTrainingCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new TrainingUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.TrainingFormDto);
            var TrainingDto = _mapper.Map<TrainingDto>(request.TrainingFormDto);
            var flag = await _trainingRepository.Exists(request.TrainingFormDto.Id);

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
                            nameof(Training), request.TrainingFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.TrainingFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.TrainingFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _trainingRepository.GetById(
                                request.TrainingFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.TrainingFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.TrainingFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            TrainingDto.ImgName = fileName;
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
                    TrainingDto.ImgName = (await _trainingRepository.GetById(
                                request.TrainingFormDto.Id)).ImgName;
                } 

                var updateData = await _trainingRepository.GetById(request.TrainingFormDto.Id);
                
                _mapper.Map(TrainingDto, updateData);

                var data = await _trainingRepository.Update(updateData);

                response.Data = _mapper.Map<TrainingDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

