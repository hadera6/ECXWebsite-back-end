using ECX.Website.Application.CQRS.Commodities.Request.Command;
using ECX.Website.Application.CQRS.Commodities.Request.Queries;
using ECX.Website.Application.DTOs.Commodity;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECX.Website.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommodityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommodityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<CommodityController>
        [HttpGet]
        public async Task<BaseCommonResponse> Get()
        {
            var query = new GetCommodityListRequest();
            BaseCommonResponse response = await _mediator.Send(query);
            return response;
        }

        // GET api/<CommodityController>/5
        [HttpGet("{id}")]
        public async Task<BaseCommonResponse> Get(string id)
        {
            var query = new GetCommodityDetailRequest { Id = id };
            BaseCommonResponse response = await _mediator.Send(query);
            return response;
        }

        // POST api/<CommodityController>
        [HttpPost]
        public async Task<BaseCommonResponse> Post([FromForm] CommodityFormDto commodity)
        {
            var command = new CreateCommodityCommand { CommodityFormDto = commodity };
            BaseCommonResponse response = await _mediator.Send(command);
            return response;

        }

        // PUT api/<CommodityController>/5
        [HttpPut]
        public async Task<BaseCommonResponse> Put([FromForm] CommodityFormDto commodity)
        {
            var command = new UpdateCommodityCommand { CommodityFormDto = commodity};
            BaseCommonResponse response = await _mediator.Send(command);
            return response;
        }

        // DELETE api/<CommodityController>/5
        [HttpDelete("{id}")]
        public async Task<BaseCommonResponse> Delete(string id)
        {
            var command = new DeleteCommodityCommand { Id = id };
            BaseCommonResponse response = await _mediator.Send(command);
            return response;
        }
    }
}
