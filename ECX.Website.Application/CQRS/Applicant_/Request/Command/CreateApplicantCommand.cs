using ECX.Website.Application.DTOs.Applicant;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Applicant_.Request.Command
{
    public class CreateApplicantCommand : IRequest<BaseCommonResponse>
    {
        public ApplicantFormDto ApplicantFormDto { get; set; }
    }
}
