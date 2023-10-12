using AutoMapper;
using ECX.Website.Application.CQRS.Applicant_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Applicant_.Handler.Command
{
    public class DeleteApplicantCommandHandler : IRequestHandler<DeleteApplicantCommand, BaseCommonResponse>
    {
        
        private IApplicantRepository _applicantRepository;
        private IMapper _mapper;
        public DeleteApplicantCommandHandler(IApplicantRepository applicantRepository, IMapper mapper)
        {
            _applicantRepository = applicantRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteApplicantCommand request, CancellationToken cancellationToken)
        {
            var data = await _applicantRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Applicant), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _applicantRepository.Delete(data);

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
