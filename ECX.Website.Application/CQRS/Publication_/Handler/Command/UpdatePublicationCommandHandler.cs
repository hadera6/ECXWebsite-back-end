using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Publication_.Request.Command;
using ECX.Website.Application.DTOs.Publication;
using ECX.Website.Application.DTOs.Publication.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Publication_.Handler.Command
{
    public class UpdatePublicationCommandHandler : IRequestHandler<UpdatePublicationCommand, BaseCommonResponse>
    {
        private IPublicationRepository _publicationRepository;
        private IMapper _mapper;
        public UpdatePublicationCommandHandler(IPublicationRepository publicationRepository, IMapper mapper)
        {
            _publicationRepository = publicationRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdatePublicationCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new PublicationUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.PublicationFormDto);
            var PublicationDto = _mapper.Map<PublicationDto>(request.PublicationFormDto);
            var flag = await _publicationRepository.Exists(request.PublicationFormDto.Id);

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
                            nameof(Publication), request.PublicationFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.PublicationFormDto.File != null)
                {
                    try
                    {
                        var pdfValidator = new PdfValidator();
                        var pdfValidationResult = await pdfValidator.ValidateAsync(request.PublicationFormDto.File);

                        if (pdfValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = pdfValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldPdf = (await _publicationRepository.GetById(
                                request.PublicationFormDto.Id)).FileName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\pdf",oldPdf);
                            File.Delete(oldPath);

                            string contentType = request.PublicationFormDto.File.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\pdf", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.PublicationFormDto.File.CopyTo(stream);
                            }
                           
                            PublicationDto.FileName = fileName;
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
                    PublicationDto.FileName = (await _publicationRepository.GetById(
                                request.PublicationFormDto.Id)).FileName;
                } 

                var updateData = await _publicationRepository.GetById(request.PublicationFormDto.Id);
                
                _mapper.Map(PublicationDto, updateData);

                var data = await _publicationRepository.Update(updateData);

                response.Data = _mapper.Map<PublicationDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

