using AutoMapper;
using ECX.Website.Application.CQRS.SocialMedia_.Request.Command;
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

namespace ECX.Website.Application.CQRS.SocialMedia_.Handler.Command
{
    public class DeleteSocialMediaCommandHandler : IRequestHandler<DeleteSocialMediaCommand, BaseCommonResponse>
    {
        
        private ISocialMediaRepository _socialMediaRepository;
        private IMapper _mapper;
        public DeleteSocialMediaCommandHandler(ISocialMediaRepository socialMediaRepository, IMapper mapper)
        {
            _socialMediaRepository = socialMediaRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteSocialMediaCommand request, CancellationToken cancellationToken)
        {
            var data = await _socialMediaRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(SocialMedia), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _socialMediaRepository.Delete(data);

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
