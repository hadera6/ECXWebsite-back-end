using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.ExternalLink_.Request.Command;
using ECX.Website.Application.DTOs.ExternalLink;
using ECX.Website.Application.DTOs.ExternalLink.Validators;
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

namespace ECX.Website.Application.CQRS.ExternalLink_.Handler.Command
{
    public class CreateExternalLinkCommandHandler : IRequestHandler<CreateExternalLinkCommand, BaseCommonResponse>
    {
        private IExternalLinkRepository _externalLinkRepository;
        private IMapper _mapper;

        public CreateExternalLinkCommandHandler(IExternalLinkRepository externalLinkRepository, IMapper mapper)
        {
            _externalLinkRepository = externalLinkRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(CreateExternalLinkCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var validator = new ExternalLinkCreateDtoValidator();
            var validationResult = await validator.ValidateAsync(request.ExternalLinkFormDto);
            var ExternalLinkDto = _mapper.Map<ExternalLinkDto>(request.ExternalLinkFormDto);
            if (validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Faild";
                response.Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                response.Status = "400";
            }
            else
            {
                string externalLinkId;
                bool flag = true;

                while (true)
                {
                    externalLinkId = Guid.NewGuid().ToString();
                    flag = await _externalLinkRepository.Exists(externalLinkId);
                    if (flag == false)
                    {
                        ExternalLinkDto.Id = externalLinkId;
                        break;
                    }
                }

                var data = _mapper.Map<ExternalLink>(ExternalLinkDto);

                var saveData = await _externalLinkRepository.Add(data);

                response.Data = _mapper.Map<ExternalLinkDto>(saveData);
                response.Success = true;
                response.Message = "Created Successfully";
                response.Status = "200";
                    
            }
            return response;
        }
    }
}
