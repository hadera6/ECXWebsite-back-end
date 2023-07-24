using AutoMapper;
using ECX.Website.Application.CQRS.Commodities.Request.Command;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Contracts.Persistence;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECX.Website.Domain;
using ECX.Website.Application.DTOs.Commodity.Validators;

namespace ECX.Website.Application.CQRS.Commodities.Handler.Command
{
    public class UpdateCommodityCommandHandler : IRequestHandler<UpdateCommodityCommand, Unit>
    {
        private ICommodityRepository _commodityRepository;
        private IMapper _mapper;
        public UpdateCommodityCommandHandler(ICommodityRepository commodityRepository, IMapper mapper)
        {
            _commodityRepository = commodityRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCommodityCommand request, CancellationToken cancellationToken)
        {
            var validator = new CommodityDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CommodityDto);
            if (validationResult.IsValid == false)
                throw new ValidationException(validationResult);
            var commodity = await _commodityRepository.GetById(request.CommodityDto.Id);
            _mapper.Map(request.CommodityDto, commodity);
            await _commodityRepository.Update(commodity);
            return Unit.Value;
        }
    }
}
