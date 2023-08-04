﻿using ECX.Website.Application.CQRS.Commodities.Request.Command;
using ECX.Website.Application.CQRS.Commodities.Request.Queries;
using ECX.Website.Application.DTOs.Commodity;
using ECX.Website.Application.Response;
using ECX.Website.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<List<CommodityDto>>> Get()
        {
            var commodities = await _mediator.Send(new GetCommodityListRequest());
            return Ok(commodities);
        }

        // GET api/<CommodityController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommodityDto>> Get(int id)
        {
            var commodity = await _mediator.Send(new GetCommodityDetailRequest { Id= id});
            return Ok(commodity);
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
        public async Task<ActionResult> Delete(int id)
        {
            var command = new DeleteCommodityCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
