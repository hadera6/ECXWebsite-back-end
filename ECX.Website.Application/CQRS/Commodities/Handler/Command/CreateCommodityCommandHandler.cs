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

namespace ECX.Website.Application.CQRS.Commodities.Handler.Command
{
    public class CreateCommodityCommandHandler : IRequestHandler<CreateCommodityCommand, BaseCommandResponse>
    {
        BaseCommandResponse response;
        private ICommodityRepository _commodityRepository;
        private IMapper _mapper;
        public CreateCommodityCommandHandler(ICommodityRepository commodityRepository, IMapper mapper)
        {
            _commodityRepository = commodityRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommandResponse> Handle(CreateCommodityCommand request, CancellationToken cancellationToken)
        {
            response = new BaseCommandResponse();
            var validator =new CommodityDtoValidator();
            var validationResult =await validator.ValidateAsync(request.CommodityDto);
            
            if(validationResult.IsValid == false)
            {
                response.Success = false;
                response.Message = "Creation Faild";
                response.Errors= validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            }
           
            var commodity = _mapper.Map<Commodity>(request.CommodityDto);
            var data =await _commodityRepository.Add(commodity);
            response.Success = true;
            response.Message = "Creation Successfull";
            return response;
        }
    }
}
