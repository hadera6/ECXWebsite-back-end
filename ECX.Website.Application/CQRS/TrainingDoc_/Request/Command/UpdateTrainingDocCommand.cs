using ECX.Website.Application.DTOs.TrainingDoc;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.TrainingDoc_.Request.Command
{
    public class UpdateTrainingDocCommand :IRequest<BaseCommonResponse>
    {
        public TrainingDocFormDto TrainingDocFormDto { get; set; }

    }
}
