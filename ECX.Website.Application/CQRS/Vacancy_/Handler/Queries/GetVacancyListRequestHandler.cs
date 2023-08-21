using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Vacancy_.Request.Command;
using ECX.Website.Application.DTOs.Vacancy;
using ECX.Website.Application.DTOs.Vacancy.Validators;
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
using ECX.Website.Application.CQRS.Vacancy_.Request.Queries;

namespace ECX.Website.Application.CQRS.Vacancy_.Handler.Queries
{
    public class GetVacancyListRequestHandler : IRequestHandler<GetVacancyListRequest, BaseCommonResponse>
    {
        private IVacancyRepository _vacancyRepository;
        private IMapper _mapper;
        public GetVacancyListRequestHandler(IVacancyRepository vacancyRepository, IMapper mapper)
        {
            _vacancyRepository = vacancyRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetVacancyListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _vacancyRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<VacancyDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
