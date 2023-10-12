using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Research_.Request.Command;
using ECX.Website.Application.DTOs.Research;
using ECX.Website.Application.DTOs.Research.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Research_.Handler.Command
{
    public class UpdateResearchCommandHandler : IRequestHandler<UpdateResearchCommand, BaseCommonResponse>
    {
        private IResearchRepository _researchRepository;
        private IMapper _mapper;
        public UpdateResearchCommandHandler(IResearchRepository researchRepository, IMapper mapper)
        {
            _researchRepository = researchRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateResearchCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new ResearchUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ResearchFormDto);
            var ResearchDto = _mapper.Map<ResearchDto>(request.ResearchFormDto);
            var flag = await _researchRepository.Exists(request.ResearchFormDto.Id);

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
                            nameof(Research), request.ResearchFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.ResearchFormDto.File != null)
                {
                    try
                    {
                        var pdfValidator = new PdfValidator();
                        var pdfValidationResult = await pdfValidator.ValidateAsync(request.ResearchFormDto.File);

                        if (pdfValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = pdfValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldPdf = (await _researchRepository.GetById(
                                request.ResearchFormDto.Id)).FileName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\pdf",oldPdf);
                            File.Delete(oldPath);

                            string contentType = request.ResearchFormDto.File.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\pdf", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.ResearchFormDto.File.CopyTo(stream);
                            }
                           
                            ResearchDto.FileName = fileName;
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
                    ResearchDto.FileName = (await _researchRepository.GetById(
                                request.ResearchFormDto.Id)).FileName;
                } 

                var updateData = await _researchRepository.GetById(request.ResearchFormDto.Id);
                
                _mapper.Map(ResearchDto, updateData);

                var data = await _researchRepository.Update(updateData);

                response.Data = _mapper.Map<ResearchDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

