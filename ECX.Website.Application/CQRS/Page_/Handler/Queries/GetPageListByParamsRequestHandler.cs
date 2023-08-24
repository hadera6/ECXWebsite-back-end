using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Page_.Request.Command;
using ECX.Website.Application.DTOs.Page;
using ECX.Website.Application.DTOs.Page.Validators;
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
using ECX.Website.Application.CQRS.Page_.Request.Queries;

namespace ECX.Website.Application.CQRS.Page_.Handler.Queries
{
    public class GetPageListByParamsRequestHandler : IRequestHandler<GetPageListByParamsRequest, BaseCommonResponse>
    {
        private IPageRepository _pageRepository;
        private IMapper _mapper;
        
        public GetPageListByParamsRequestHandler(IPageRepository pageRepository, IMapper mapper)
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetPageListByParamsRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _pageRepository.GetByPageCatagoryLangId(request.CatagoryId,request.LangId);
            
            
                response.Success = true;
                response.Data = _mapper.Map<List<PageDto>>(data);
                response.Status = "200";

            return response;
        }
    }
}
