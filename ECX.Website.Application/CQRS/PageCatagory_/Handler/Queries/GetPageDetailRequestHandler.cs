using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.PageCatagory_.Request.Command;
using ECX.Website.Application.DTOs.PageCatagory;
using ECX.Website.Application.DTOs.PageCatagory.Validators;
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
using ECX.Website.Application.CQRS.PageCatagory_.Request.Queries;

namespace ECX.Website.Application.CQRS.PageCatagory_.Handler.Queries
{
    public class GetPageCatagoryDetailRequestHandler : IRequestHandler<GetPageCatagoryDetailRequest, BaseCommonResponse>
    {
        private IPageCatagoryRepository _pageCatagoryRepository;
        private IMapper _mapper;
        
        public GetPageCatagoryDetailRequestHandler(IPageCatagoryRepository pageCatagoryRepository, IMapper mapper)
        {
            _pageCatagoryRepository = pageCatagoryRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetPageCatagoryDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _pageCatagoryRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<PageCatagoryDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                    nameof(PageCatagory), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
