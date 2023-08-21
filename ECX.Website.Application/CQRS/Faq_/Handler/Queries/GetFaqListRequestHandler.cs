using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Faq_.Request.Command;
using ECX.Website.Application.DTOs.Faq;
using ECX.Website.Application.DTOs.Faq.Validators;
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
using ECX.Website.Application.CQRS.Faq_.Request.Queries;

namespace ECX.Website.Application.CQRS.Faq_.Handler.Queries
{
    public class GetFaqListRequestHandler : IRequestHandler<GetFaqListRequest, BaseCommonResponse>
    {
        private IFaqRepository _faqRepository;
        private IMapper _mapper;
        public GetFaqListRequestHandler(IFaqRepository faqRepository, IMapper mapper)
        {
            _faqRepository = faqRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetFaqListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _faqRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<FaqDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
