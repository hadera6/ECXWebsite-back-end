using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.ContractFile_.Request.Command;
using ECX.Website.Application.DTOs.ContractFile;
using ECX.Website.Application.DTOs.ContractFile.Validators;
using ECX.Website.Application.Exceptions;

using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using ECX.Website.Application.DTOs.Common.Validators;

namespace ECX.Website.Application.CQRS.ContractFile_.Handler.Command
{
    public class CreateContractFileCommandHandler : IRequestHandler<CreateContractFileCommand, BaseCommonResponse>
    {
        private IContractFileRepository _contractFileRepository;
        private IMapper _mapper;

        public CreateContractFileCommandHandler(IContractFileRepository contractFileRepository, IMapper mapper)
        {
            _contractFileRepository = contractFileRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateContractFileCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new ContractFileCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ContractFileFormDto);

            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Faild";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else
            {
                try
                {
                    var imageValidator = new ImageValidator();
                    var imgValidationResult = await imageValidator.ValidateAsync(request.ContractFileFormDto.ImgFile);

                    if (imgValidationResult.IsValid == false)
                    {
                        response.Success = false;
                        response.Message = "Creation Faild";
                        response.Errors = imgValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
                        response.Status = "400";
                    }
                    else
                    {
                        string contentType = request.ContractFileFormDto.ImgFile.ContentType.ToString();
                        string ext = contentType.Split('/')[1];
                        string fileName = Guid.NewGuid().ToString() + "." + ext;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\image", fileName);

                        using (Stream stream = new FileStream(path, FileMode.Create))
                        {
                            request.ContractFileFormDto.ImgFile.CopyTo(stream);
                        }
                        var ContractFileDto = _mapper.Map<ContractFileDto>(request.ContractFileFormDto);
                        ContractFileDto.ImgName = fileName;

                        string contractFileId;
                        bool flag = true;

                        while (true)
                        {
                            contractFileId = Guid.NewGuid().ToString();
                            flag = await _contractFileRepository.Exists(contractFileId);
                            if (flag == false)
                            {
                                ContractFileDto.Id = contractFileId;
                                break;
                            }
                        }

                        var data = _mapper.Map<ContractFile>(ContractFileDto);

                        var saveData = await _contractFileRepository.Add(data);

                        response.Data = _mapper.Map<ContractFileDto>(saveData);
                        response.Success = true;
                        response.Message = "Created Successfully";
                        response.Status = "200";
                    }
                }
                catch (Exception ex)
                {
                    response.Success = false;
                    response.Message = "Creation Failed";
                    response.Errors = new List<string> { ex.Message };
                    response.Status = "400";
                }
            }
            return response;
        }
    }
}
