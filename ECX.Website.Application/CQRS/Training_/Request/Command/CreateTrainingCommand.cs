using ECX.Website.Application.DTOs.Training;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.Training_.Request.Command
{
    public class CreateTrainingCommand : IRequest<BaseCommonResponse>
    {
        public TrainingFormDto TrainingFormDto { get; set; }
    }
}
