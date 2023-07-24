using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Commodities.Request.Command;
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
using ECX.Website.Application.CQRS.Commodities.Request.Queries;

namespace ECX.Website.Application.CQRS.Commodities.Handler.Queries
{
    public class GetCommodityDetailRequestHandler : IRequestHandler<GetCommodityDetailRequest, CommodityDto>
    {
        private ICommodityRepository _commodityRepository;
        private IMapper _mapper;
        public GetCommodityDetailRequestHandler(ICommodityRepository commodityRepository, IMapper mapper)
        {
            _commodityRepository = commodityRepository;
            _mapper = mapper;
        }
        public async Task<CommodityDto> Handle(GetCommodityDetailRequest request, CancellationToken cancellationToken)
        {
            var commodity = await _commodityRepository.GetById(request.Id);
            return _mapper.Map<CommodityDto>(commodity);
        }
    }
}
