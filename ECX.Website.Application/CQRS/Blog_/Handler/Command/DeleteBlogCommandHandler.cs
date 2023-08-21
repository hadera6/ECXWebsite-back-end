using AutoMapper;
using ECX.Website.Application.CQRS.Blog_.Request.Command;
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

namespace ECX.Website.Application.CQRS.Blog_.Handler.Command
{
    public class DeleteBlogCommandHandler : IRequestHandler<DeleteBlogCommand, BaseCommonResponse>
    {
        
        private IBlogRepository _blogRepository;
        private IMapper _mapper;
        public DeleteBlogCommandHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
            var data = await _blogRepository.GetById(request.Id);
            var response = new BaseCommonResponse();

            if (data == null)
            {
                response.Success = false;
                response.Message = new NotFoundException(
                            nameof(Blog), request.Id).Message.ToString();
                response.Status = "404";
            }
            else
            {
                await _blogRepository.Delete(data);

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
