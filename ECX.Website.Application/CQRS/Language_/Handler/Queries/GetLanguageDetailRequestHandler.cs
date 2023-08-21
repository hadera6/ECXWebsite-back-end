using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Language_.Request.Command;
using ECX.Website.Application.DTOs.Language;
using ECX.Website.Application.DTOs.Language.Validators;
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
using ECX.Website.Application.CQRS.Language_.Request.Queries;

namespace ECX.Website.Application.CQRS.Language_.Handler.Queries
{
    public class GetLanguageDetailRequestHandler : IRequestHandler<GetLanguageDetailRequest, BaseCommonResponse>
    {
        private ILanguageRepository _languageRepository;
        private IMapper _mapper;
        
        public GetLanguageDetailRequestHandler(ILanguageRepository languageRepository, IMapper mapper)
        {
            _languageRepository = languageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetLanguageDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _languageRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<LanguageDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(Language), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
