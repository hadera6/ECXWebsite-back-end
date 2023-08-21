using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.TrainingDoc_.Request.Command;
using ECX.Website.Application.DTOs.TrainingDoc;
using ECX.Website.Application.DTOs.TrainingDoc.Validators;
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
using ECX.Website.Application.CQRS.TrainingDoc_.Request.Queries;

namespace ECX.Website.Application.CQRS.TrainingDoc_.Handler.Queries
{
    public class GetTrainingDocDetailRequestHandler : IRequestHandler<GetTrainingDocDetailRequest, BaseCommonResponse>
    {
        private ITrainingDocRepository _trainingDocRepository;
        private IMapper _mapper;
        
        public GetTrainingDocDetailRequestHandler(ITrainingDocRepository trainingDocRepository, IMapper mapper)
        {
            _trainingDocRepository = trainingDocRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetTrainingDocDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _trainingDocRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<TrainingDocDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(TrainingDoc), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
