using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Brochure_.Request.Command;
using ECX.Website.Application.DTOs.Brochure;
using ECX.Website.Application.DTOs.Brochure.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.Brochure_.Handler.Command
{
    public class UpdateBrochureCommandHandler : IRequestHandler<UpdateBrochureCommand, BaseCommonResponse>
    {
        private IBrochureRepository _brochureRepository;
        private IMapper _mapper;
        public UpdateBrochureCommandHandler(IBrochureRepository brochureRepository, IMapper mapper)
        {
            _brochureRepository = brochureRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateBrochureCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new BrochureUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.BrochureFormDto);
            var BrochureDto = _mapper.Map<BrochureDto>(request.BrochureFormDto);
            var flag = await _brochureRepository.Exists(request.BrochureFormDto.Id);

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
                            nameof(Brochure), request.BrochureFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.BrochureFormDto.File != null)
                {
                    try
                    {
                        var pdfValidator = new PdfValidator();
                        var pdfValidationResult = await pdfValidator.ValidateAsync(request.BrochureFormDto.File);

                        if (pdfValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = pdfValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldPdf = (await _brochureRepository.GetById(
                                request.BrochureFormDto.Id)).FileName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\pdf",oldPdf);
                            File.Delete(oldPath);

                            string contentType = request.BrochureFormDto.File.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\pdf", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.BrochureFormDto.File.CopyTo(stream);
                            }
                           
                            BrochureDto.FileName = fileName;
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
                    BrochureDto.FileName = (await _brochureRepository.GetById(
                                request.BrochureFormDto.Id)).FileName;
                } 

                var updateData = await _brochureRepository.GetById(request.BrochureFormDto.Id);
                
                _mapper.Map(BrochureDto, updateData);

                var data = await _brochureRepository.Update(updateData);

                response.Data = _mapper.Map<BrochureDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

