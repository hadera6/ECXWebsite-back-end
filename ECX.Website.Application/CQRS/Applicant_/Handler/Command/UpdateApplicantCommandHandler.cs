using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Applicant_.Request.Command;
using ECX.Website.Application.DTOs.Applicant;
using ECX.Website.Application.DTOs.Applicant.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Applicant_.Handler.Command
{
    public class UpdateApplicantCommandHandler : IRequestHandler<UpdateApplicantCommand, BaseCommonResponse>
    {
        private IApplicantRepository _applicantRepository;
        private IMapper _mapper;
        public UpdateApplicantCommandHandler(IApplicantRepository applicantRepository, IMapper mapper)
        {
            _applicantRepository = applicantRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateApplicantCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new ApplicantUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ApplicantFormDto);
            var ApplicantDto = _mapper.Map<ApplicantDto>(request.ApplicantFormDto);
            var flag = await _applicantRepository.Exists(request.ApplicantFormDto.Id);

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
                            nameof(Applicant), request.ApplicantFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.ApplicantFormDto.File != null)
                {
                    try
                    {
                        var pdfValidator = new PdfValidator();
                        var pdfValidationResult = await pdfValidator.ValidateAsync(request.ApplicantFormDto.File);

                        if (pdfValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = pdfValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldPdf = (await _applicantRepository.GetById(
                                request.ApplicantFormDto.Id)).FileName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\pdf",oldPdf);
                            File.Delete(oldPath);

                            string contentType = request.ApplicantFormDto.File.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\pdf", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.ApplicantFormDto.File.CopyTo(stream);
                            }
                           
                            ApplicantDto.FileName = fileName;
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
                    ApplicantDto.FileName = (await _applicantRepository.GetById(
                                request.ApplicantFormDto.Id)).FileName;
                } 

                var updateData = await _applicantRepository.GetById(request.ApplicantFormDto.Id);
                
                _mapper.Map(ApplicantDto, updateData);

                var data = await _applicantRepository.Update(updateData);

                response.Data = _mapper.Map<ApplicantDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

