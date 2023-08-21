using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Training_.Request.Command;
using ECX.Website.Application.DTOs.Training;
using ECX.Website.Application.DTOs.Training.Validators;
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
using ECX.Website.Application.CQRS.Training_.Request.Queries;

namespace ECX.Website.Application.CQRS.Training_.Handler.Queries
{
    public class GetTrainingListRequestHandler : IRequestHandler<GetTrainingListRequest, BaseCommonResponse>
    {
        private ITrainingRepository _trainingRepository;
        private IMapper _mapper;
        public GetTrainingListRequestHandler(ITrainingRepository trainingRepository, IMapper mapper)
        {
            _trainingRepository = trainingRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetTrainingListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _trainingRepository.GetAll();

            response.Success = true;
            response.Data = _mapper.Map<List<TrainingDto>>(data);
            response.Status = "200";

            return response;
        }
    }
}
