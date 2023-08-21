using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.WareHouse_.Request.Command;
using ECX.Website.Application.DTOs.WareHouse;
using ECX.Website.Application.DTOs.WareHouse.Validators;
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
using ECX.Website.Application.CQRS.WareHouse_.Request.Queries;

namespace ECX.Website.Application.CQRS.WareHouse_.Handler.Queries
{
    public class GetWareHouseListRequestHandler : IRequestHandler<GetWareHouseListRequest, BaseCommonResponse>
    {
        private IWareHouseRepository _wareHouseRepository;
        private IMapper _mapper;
        public GetWareHouseListRequestHandler(IWareHouseRepository wareHouseRepository, IMapper mapper)
        {
            _wareHouseRepository = wareHouseRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetWareHouseListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _wareHouseRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<WareHouseDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
