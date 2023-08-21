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
    public class GetTradingCenterDetailRequestHandler : IRequestHandler<GetTradingCenterDetailRequest, BaseCommonResponse>
    {
        private ITradingCenterRepository _tradingCenterRepository;
        private IMapper _mapper;
        
        public GetTradingCenterDetailRequestHandler(ITradingCenterRepository tradingCenterRepository, IMapper mapper)
        {
            _tradingCenterRepository = tradingCenterRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetTradingCenterDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _tradingCenterRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<TradingCenterDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(TradingCenter), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
