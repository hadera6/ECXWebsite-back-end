using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.ContractFile_.Request.Command;
using ECX.Website.Application.DTOs.ContractFile;
using ECX.Website.Application.DTOs.ContractFile.Validators;
using ECX.Website.Application.DTOs.Common.Validators;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;

namespace ECX.Website.Application.CQRS.ContractFile_.Handler.Command
{
    public class UpdateContractFileCommandHandler : IRequestHandler<UpdateContractFileCommand, BaseCommonResponse>
    {
        private IContractFileRepository _contractFileRepository;
        private IMapper _mapper;
        public UpdateContractFileCommandHandler(IContractFileRepository ContractFileRepository, IMapper mapper)
        {
            _contractFileRepository = ContractFileRepository;
            _mapper = mapper;
        }

        public async Task<BaseCommonResponse> Handle(UpdateContractFileCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new ContractFileUpdateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ContractFileFormDto);
            var ContractFileDto = _mapper.Map<ContractFileDto>(request.ContractFileFormDto);
            var flag = await _contractFileRepository.Exists(request.ContractFileFormDto.Id);

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
                            nameof(ContractFile), request.ContractFileFormDto.Id).Message.ToString();
                response.Status = "404";
            }
            else 
            {
                if (request.ContractFileFormDto.File != null)
                {
                    try
                    {
                        var pdfValidator = new PdfValidator();
                        var pdfValidationResult = await pdfValidator.ValidateAsync(request.ContractFileFormDto.File);

                        if (pdfValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = pdfValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldPdf = (await _contractFileRepository.GetById(
                                request.ContractFileFormDto.Id)).FileName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\pdf",oldPdf);
                            File.Delete(oldPath);

                            string contentType = request.ContractFileFormDto.File.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\pdf", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.ContractFileFormDto.File.CopyTo(stream);
                            }
                           
                            ContractFileDto.FileName = fileName;
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
                    ContractFileDto.FileName = (await _contractFileRepository.GetById(
                                request.ContractFileFormDto.Id)).FileName;
                } 

                var updateData = await _contractFileRepository.GetById(request.ContractFileFormDto.Id);
                
                _mapper.Map(ContractFileDto, updateData);

                var data = await _contractFileRepository.Update(updateData);

                response.Data = _mapper.Map<ContractFileDto>(data);
                response.Success = true;
                response.Message = "Updated Successfull";
                response.Status = "200";
            }
            return response;
        }
    }
 }

