using ECX.Website.Application.DTOs.Vacancy;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Vacancy_.Request.Command
{
    public class CreateVacancyCommand : IRequest<BaseCommonResponse>
    {
        public VacancyFormDto VacancyFormDto { get; set; }
    }
}
