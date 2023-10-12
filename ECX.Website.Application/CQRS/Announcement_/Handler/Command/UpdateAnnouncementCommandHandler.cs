using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Announcement_.Request.Command;
using ECX.Website.Application.DTOs.Announcement;
using ECX.Website.Application.DTOs.Announcement.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Announcement_.Handler.Command
{
    public class UpdateAnnouncementCommandHandler : IRequestHandler<UpdateAnnouncementCommand, BaseCommonResponse>
    {
        private IAnnouncementRepository _announcementRepository;
        private IMapper _mapper;
        public UpdateAnnouncementCommandHandler(IAnnouncementRepository announcementRepository, IMapper mapper)
        {
            _announcementRepository = announcementRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateAnnouncementCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new AnnouncementUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.AnnouncementFormDto);
            var AnnouncementDto = _mapper.Map<AnnouncementDto>(request.AnnouncementFormDto);
            var flag = await _announcementRepository.Exists(request.AnnouncementFormDto.Id);

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
                            nameof(Announcement), request.AnnouncementFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.AnnouncementFormDto.File != null)
                {
                    try
                    {
                        var pdfValidator = new PdfValidator();
                        var pdfValidationResult = await pdfValidator.ValidateAsync(request.AnnouncementFormDto.File);

                        if (pdfValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = pdfValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldPdf = (await _announcementRepository.GetById(
                                request.AnnouncementFormDto.Id)).FileName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\pdf",oldPdf);
                            File.Delete(oldPath);

                            string contentType = request.AnnouncementFormDto.File.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\pdf", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.AnnouncementFormDto.File.CopyTo(stream);
                            }
                           
                            AnnouncementDto.FileName = fileName;
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
                    AnnouncementDto.FileName = (await _announcementRepository.GetById(
                                request.AnnouncementFormDto.Id)).FileName;
                } 

                var updateData = await _announcementRepository.GetById(request.AnnouncementFormDto.Id);
                
                _mapper.Map(AnnouncementDto, updateData);

                var data = await _announcementRepository.Update(updateData);

                response.Data = _mapper.Map<AnnouncementDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

