using AutoMapper;
using ECX.Website.Application.CQRS.Training_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Training_.Handler.Command
{
    public class DeleteTrainingCommandHandler : IRequestHandler<DeleteTrainingCommand, BaseCommonResponse>
    {
        
        private ITrainingRepository _trainingRepository;
        private IMapper _mapper;
        public DeleteTrainingCommandHandler(ITrainingRepository trainingRepository, IMapper mapper)
        {
            _trainingRepository = trainingRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteTrainingCommand request, CancellationToken cancellationToken)
        {
            var data = await _trainingRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Training), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _trainingRepository.Delete(data);

                string path = Path.Combine(
                    Directory.GetCurrentDirectory(), @"wwwroot\image", data.ImgName);

                File.Delete(path);

                response.Success = true;
                response.Message = "Successfully Deleted";
                response.Status = "200";

            }
                
            return response;
        }
    }
}
