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
    public class GetTrainingDetailRequestHandler : IRequestHandler<GetTrainingDetailRequest, BaseCommonResponse>
    {
        private ITrainingRepository _trainingRepository;
        private IMapper _mapper;
        
        public GetTrainingDetailRequestHandler(ITrainingRepository trainingRepository, IMapper mapper)
        {
            _trainingRepository = trainingRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetTrainingDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _trainingRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<TrainingDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(Training), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
