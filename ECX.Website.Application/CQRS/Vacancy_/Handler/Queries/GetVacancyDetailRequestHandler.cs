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
    public class GetVacancyDetailRequestHandler : IRequestHandler<GetVacancyDetailRequest, BaseCommonResponse>
    {
        private IVacancyRepository _vacancyRepository;
        private IMapper _mapper;
        
        public GetVacancyDetailRequestHandler(IVacancyRepository vacancyRepository, IMapper mapper)
        {
            _vacancyRepository = vacancyRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetVacancyDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _vacancyRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<VacancyDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(Vacancy), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
