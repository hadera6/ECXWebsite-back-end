using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Brochure_.Request.Command;
using ECX.Website.Application.DTOs.Brochure;
using ECX.Website.Application.DTOs.Brochure.Validators;
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
using ECX.Website.Application.CQRS.Brochure_.Request.Queries;

namespace ECX.Website.Application.CQRS.Brochure_.Handler.Queries
{
    public class GetBrochureDetailRequestHandler : IRequestHandler<GetBrochureDetailRequest, BaseCommonResponse>
    {
        private IBrochureRepository _brochureRepository;
        private IMapper _mapper;
        
        public GetBrochureDetailRequestHandler(IBrochureRepository brochureRepository, IMapper mapper)
        {
            _brochureRepository = brochureRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetBrochureDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _brochureRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<BrochureDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(Brochure), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
