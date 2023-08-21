using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Applicant_.Request.Command;
using ECX.Website.Application.DTOs.Applicant;
using ECX.Website.Application.DTOs.Applicant.Validators;
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
using ECX.Website.Application.CQRS.Applicant_.Request.Queries;

namespace ECX.Website.Application.CQRS.Applicant_.Handler.Queries
{
    public class GetApplicantListRequestHandler : IRequestHandler<GetApplicantListRequest, BaseCommonResponse>
    {
        private IApplicantRepository _applicantRepository;
        private IMapper _mapper;
        public GetApplicantListRequestHandler(IApplicantRepository applicantRepository, IMapper mapper)
        {
            _applicantRepository = applicantRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetApplicantListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _applicantRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<ApplicantDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
