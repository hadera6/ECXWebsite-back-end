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

namespace ECX.Website.Application.CQRS.Commodities.Handler.Command
{
    public class DeleteCommodityCommandHandler : IRequestHandler<DeleteCommodityCommand>
    {
        private ICommodityRepository _commodityRepository;
        private IMapper _mapper;
        public DeleteCommodityCommandHandler(ICommodityRepository commodityRepository, IMapper mapper)
        {
            _commodityRepository = commodityRepository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(DeleteCommodityCommand request, CancellationToken cancellationToken)
        {
            var commodity = await _commodityRepository.GetById(request.Id);
            if(commodity == null) 
                throw new NotFoundException(nameof(Commodity), request.Id);
            await _commodityRepository.Delete(commodity);

            return Unit.Value;
        }
    }
}
