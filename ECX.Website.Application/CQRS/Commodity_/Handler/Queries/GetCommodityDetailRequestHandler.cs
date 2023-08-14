using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Commodity_.Request.Command;
using ECX.Website.Application.DTOs.Commodity;
using ECX.Website.Application.DTOs.Commodity.Validators;
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
using ECX.Website.Application.CQRS.Commodity_.Request.Queries;

namespace ECX.Website.Application.CQRS.Commodity_.Handler.Queries
{
    public class GetCommodityDetailRequestHandler : IRequestHandler<GetCommodityDetailRequest, BaseCommonResponse>
    {
        private ICommodityRepository _commodityRepository;
        private IMapper _mapper;
        
        public GetCommodityDetailRequestHandler(ICommodityRepository commodityRepository, IMapper mapper)
        {
            _commodityRepository = commodityRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetCommodityDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _commodityRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<CommodityDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(Commodity), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
