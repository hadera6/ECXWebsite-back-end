using ECX.Website.Application.CQRS.PageCatagory_.Request.Command;
using ECX.Website.Application.CQRS.PageCatagory_.Request.Queries;
using ECX.Website.Application.DTOs.PageCatagory;
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
    public class PageCatagoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PageCatagoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<PageCatagoryController>
        [HttpGet]
        public async Task<ActionResult<BaseCommonResponse>> Get()
        {
            var query = new GetPageCatagoryListRequest();
            BaseCommonResponse response = await _mediator.Send(query);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;  
            }
        }

        // GET api/<PageCatagoryController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseCommonResponse>> Get(string id)
        {
            var query = new GetPageCatagoryDetailRequest { Id = id };
            BaseCommonResponse response = await _mediator.Send(query);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;  
            }
        }

        // POST api/<PageCatagoryController>
        [HttpPost]
        public async Task<ActionResult<BaseCommonResponse>> Post([FromForm] PageCatagoryFormDto data)
        {
            var command = new CreatePageCatagoryCommand { PageCatagoryFormDto = data };
            BaseCommonResponse response = await _mediator.Send(command);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;
                
            }

        }

        // PUT api/<PageCatagoryController>/5
        [HttpPut]
        public async Task<ActionResult<BaseCommonResponse>> Put([FromForm] PageCatagoryFormDto data)
        {
            var command = new UpdatePageCatagoryCommand { PageCatagoryFormDto = data};
            BaseCommonResponse response = await _mediator.Send(command);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;
                
            }
        }

        // DELETE api/<PageCatagoryController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseCommonResponse>> Delete(string id)
        {
            var command = new DeletePageCatagoryCommand { Id = id };
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
