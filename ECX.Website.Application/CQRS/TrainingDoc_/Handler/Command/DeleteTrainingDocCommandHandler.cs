using AutoMapper;
using ECX.Website.Application.CQRS.TrainingDoc_.Request.Command;
using ECX.Website.Application.Exceptions;
using ECX.Website.Application.Contracts.Persistence;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ECX.Website.Domain;
using ECX.Website.Application.Response;

namespace ECX.Website.Application.CQRS.TrainingDoc_.Handler.Command
{
    public class DeleteTrainingDocCommandHandler : IRequestHandler<DeleteTrainingDocCommand, BaseCommonResponse>
    {
        
        private ITrainingDocRepository _trainingDocRepository;
        private IMapper _mapper;
        public DeleteTrainingDocCommandHandler(ITrainingDocRepository trainingDocRepository, IMapper mapper)
        {
            _trainingDocRepository = trainingDocRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteTrainingDocCommand request, CancellationToken cancellationToken)
        {
            var data = await _trainingDocRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(TrainingDoc), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _trainingDocRepository.Delete(data);

                string path = Path.Combine(
                    Directory.GetCurrentDirectory(), @"wwwroot\pdf", data.FileName);

                File.Delete(path);

                response.Success = true;
                response.Message = "Successfully Deleted";
                response.Status = "200";

            }
                
            return response;
        }
    }
}
