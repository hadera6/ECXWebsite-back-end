using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.TradingCenter_.Request.Command;
using ECX.Website.Application.DTOs.TradingCenter;
using ECX.Website.Application.DTOs.TradingCenter.Validators;
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
using ECX.Website.Application.CQRS.TradingCenter_.Request.Queries;

namespace ECX.Website.Application.CQRS.TradingCenter_.Handler.Queries
{
    public class GetTradingCenterListRequestHandler : IRequestHandler<GetTradingCenterListRequest, BaseCommonResponse>
    {
        private ITradingCenterRepository _tradingCenterRepository;
        private IMapper _mapper;
        public GetTradingCenterListRequestHandler(ITradingCenterRepository tradingCenterRepository, IMapper mapper)
        {
            _tradingCenterRepository = tradingCenterRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetTradingCenterListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _tradingCenterRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<TradingCenterDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
