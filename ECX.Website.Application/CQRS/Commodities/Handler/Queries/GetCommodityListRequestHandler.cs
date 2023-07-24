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
    public class GetCommodityListRequestHandler : IRequestHandler<GetCommodityListRequest, List<CommodityDto>>
    {
        private ICommodityRepository _commodityRepository;
        private IMapper _mapper;
        public GetCommodityListRequestHandler(ICommodityRepository commodityRepository, IMapper mapper)
        {
            _commodityRepository = commodityRepository;
            _mapper = mapper;
        }
        public async Task<List<CommodityDto>> Handle(GetCommodityListRequest request, CancellationToken cancellationToken)
        {
            var commodity = await _commodityRepository.GetAll();
            return _mapper.Map<List<CommodityDto>>(commodity);
        }
    }
}
