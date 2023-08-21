using AutoMapper;
using ECX.Website.Application.Contracts.Persistence;
using ECX.Website.Application.CQRS.Blog_.Request.Command;
using ECX.Website.Application.DTOs.Blog;
using ECX.Website.Application.DTOs.Blog.Validators;
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
using ECX.Website.Application.CQRS.Blog_.Request.Queries;

namespace ECX.Website.Application.CQRS.Blog_.Handler.Queries
{
    public class GetBlogDetailRequestHandler : IRequestHandler<GetBlogDetailRequest, BaseCommonResponse>
    {
        private IBlogRepository _blogRepository;
        private IMapper _mapper;
        
        public GetBlogDetailRequestHandler(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(GetBlogDetailRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommonResponse();
            var data = await _blogRepository.GetById(request.Id);
            if (data != null)
            {
                response.Success = true;
                response.Data = _mapper.Map<BlogDto>(data);
                response.Status = "200";
            }
            else
            {
                response.Success = false;
                response.Message = new NotFoundException(
                          nameof(Blog), request.Id).Message.ToString();
                response.Status = "404";
            }
            return response;
        }
    }
}
