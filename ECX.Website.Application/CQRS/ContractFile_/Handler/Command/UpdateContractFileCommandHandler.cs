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
        public UpdateContractFileCommandHandler(IContractFileRepository contractFileRepository, IMapper mapper)
        {
            _contractFileRepository = contractFileRepository;
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
                if (request.ContractFileFormDto.ImgFile != null)
                {
                    try
                    {
                        var imageValidator = new ImageValidator();
                        var imgValidationResult = await imageValidator.ValidateAsync(request.ContractFileFormDto.ImgFile);

                        if (imgValidationResult.IsValid == false)
                        {
                            response.Success = false;
                            response.Message = "Update Failed";
                            response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                            response.Status = "400";
                        }
                        else
                        {
                            var oldImage = (await _contractFileRepository.GetById(
                                request.ContractFileFormDto.Id)).ImgName;
                            

                            string oldPath = Path.Combine(
                                Directory.GetCurrentDirectory(), @"wwwroot\image",oldImage);
                            File.Delete(oldPath);

                            string contentType = request.ContractFileFormDto.ImgFile.ContentType.ToString();
                            string ext = contentType.Split('/')[1];
                            string fileName = Guid.NewGuid().ToString() + "." + ext;
                            string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                            using (Stream stream = new FileStream(path, FileMode.Create))
                            {
                                request.ContractFileFormDto.ImgFile.CopyTo(stream);
                            }
                           
                            ContractFileDto.ImgName = fileName;
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
                    ContractFileDto.ImgName = (await _contractFileRepository.GetById(
                                request.ContractFileFormDto.Id)).ImgName;
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

