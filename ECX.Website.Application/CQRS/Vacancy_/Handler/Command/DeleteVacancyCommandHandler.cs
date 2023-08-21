using AutoMapper;
using ECX.Website.Application.CQRS.Vacancy_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Vacancy_.Handler.Command
{
    public class DeleteVacancyCommandHandler : IRequestHandler<DeleteVacancyCommand, BaseCommonResponse>
    {
        
        private IVacancyRepository _vacancyRepository;
        private IMapper _mapper;
        public DeleteVacancyCommandHandler(IVacancyRepository vacancyRepository, IMapper mapper)
        {
            _vacancyRepository = vacancyRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteVacancyCommand request, CancellationToken cancellationToken)
        {
            var data = await _vacancyRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Vacancy), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _vacancyRepository.Delete(data);

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
