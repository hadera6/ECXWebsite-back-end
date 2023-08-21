using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Vacancy_.Request.Command;
using ECX.Website.Application.DTOs.Vacancy;
using ECX.Website.Application.DTOs.Vacancy.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Vacancy_.Handler.Command
{
    public class UpdateVacancyCommandHandler : IRequestHandler<UpdateVacancyCommand, BaseCommonResponse>
    {
        private IVacancyRepository _vacancyRepository;
        private IMapper _mapper;
        public UpdateVacancyCommandHandler(IVacancyRepository vacancyRepository, IMapper mapper)
        {
            _vacancyRepository = vacancyRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateVacancyCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new VacancyUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.VacancyFormDto);
            var VacancyDto = _mapper.Map<VacancyDto>(request.VacancyFormDto);
            var flag = await _vacancyRepository.Exists(request.VacancyFormDto.Id);

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
                            nameof(Vacancy), request.VacancyFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.VacancyFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.VacancyFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _vacancyRepository.GetById(
                                request.VacancyFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.VacancyFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.VacancyFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            VacancyDto.ImgName = fileName;
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
                    VacancyDto.ImgName = (await _vacancyRepository.GetById(
                                request.VacancyFormDto.Id)).ImgName;
                } 

                var updateData = await _vacancyRepository.GetById(request.VacancyFormDto.Id);
                
                _mapper.Map(VacancyDto, updateData);

                var data = await _vacancyRepository.Update(updateData);

                response.Data = _mapper.Map<VacancyDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

