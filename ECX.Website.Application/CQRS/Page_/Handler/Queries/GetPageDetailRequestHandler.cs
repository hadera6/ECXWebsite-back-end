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
    public class GetPageDetailRequestHandler : IRequestHandler<GetPageDetailRequest, BaseCommonResponse>
    {
        private IPageRepository _pageRepository;
        private IMapper _mapper;
        
        public GetPageDetailRequestHandler(IPageRepository pageRepository, IMapper mapper)
        {
            _pageRepository = pageRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetPageDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _pageRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<PageDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                    nameof(Page), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
