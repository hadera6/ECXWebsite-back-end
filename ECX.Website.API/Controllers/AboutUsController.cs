﻿using ECX.Website.Application.CQRS.Page_.Request.Command;
using ECX.Website.Application.CQRS.Page_.Request.Queries;
using ECX.Website.Application.DTOs.Page;
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
    public class AboutEcxController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AboutEcxController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<AboutEcxController>
        [HttpGet]
        public async Task<ActionResult<BaseCommonResponse>> Get()
        {
            var query = new GetPageListRequest();
            BaseCommonResponse response = await _mediator.Send(query);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;  
            }
        }

        // GET api/<AboutEcx>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseCommonResponse>> Get(string id)
        {
            var query = new GetPageDetailRequest { Id = id };
            BaseCommonResponse response = await _mediator.Send(query);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;  
            }
        }

        // POST api/<AboutEcx>
        [HttpPost]
        public async Task<ActionResult<BaseCommonResponse>> Post([FromForm] PageFormDto data)
        {
            var command = new CreatePageCommand { PageFormDto = data };
            BaseCommonResponse response = await _mediator.Send(command);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;
                
            }

        }

        // PUT api/<AboutEcx>/5
        [HttpPut]
        public async Task<ActionResult<BaseCommonResponse>> Put([FromForm] PageFormDto data)
        {
            var command = new UpdatePageCommand { PageFormDto = data};
            BaseCommonResponse response = await _mediator.Send(command);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;
                
            }
        }

        // DELETE api/<AboutEcx>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseCommonResponse>> Delete(string id)
        {
            var command = new DeletePageCommand { Id = id };
            BaseCommonResponse response = await _mediator.Send(command);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;
                
            }
        }
    }
}
