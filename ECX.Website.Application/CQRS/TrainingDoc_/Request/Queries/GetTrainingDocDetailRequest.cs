using ECX.Website.Application.DTOs.TrainingDoc;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.TrainingDoc_.Request.Queries
{
    public class GetTrainingDocDetailRequest :IRequest<BaseCommonResponse>
    {
        public string Id { get; set; }
    }
}
