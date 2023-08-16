using ECX.Website.Application.CQRS.BoardOfDirector_.Request.Command;
using ECX.Website.Application.CQRS.BoardOfDirector_.Request.Queries;
using ECX.Website.Application.DTOs.BoardOfDirector;
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
    public class BoardOfDirectorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BoardOfDirectorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<BoardOfDirectorController>
        [HttpGet]
        public async Task<ActionResult<BaseCommonResponse>> Get()
        {
            var query = new GetBoardOfDirectorListRequest();
            BaseCommonResponse response = await _mediator.Send(query);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;  
            }
        }

        // GET api/<BoardOfDirectorController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseCommonResponse>> Get(string id)
        {
            var query = new GetBoardOfDirectorDetailRequest { Id = id };
            BaseCommonResponse response = await _mediator.Send(query);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;  
            }
        }

        // POST api/<BoardOfDirectorController>
        [HttpPost]
        public async Task<ActionResult<BaseCommonResponse>> Post([FromForm] BoardOfDirectorFormDto data)
        {
            var command = new CreateBoardOfDirectorCommand { BoardOfDirectorFormDto = data };
            BaseCommonResponse response = await _mediator.Send(command);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;
                
            }

        }

        // PUT api/<BoardOfDirectorController>/5
        [HttpPut]
        public async Task<ActionResult<BaseCommonResponse>> Put([FromForm] BoardOfDirectorFormDto data)
        {
            var command = new UpdateBoardOfDirectorCommand { BoardOfDirectorFormDto = data};
            BaseCommonResponse response = await _mediator.Send(command);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;
                
            }
        }

        // DELETE api/<BoardOfDirectorController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseCommonResponse>> Delete(string id)
        {
            var command = new DeleteBoardOfDirectorCommand { Id = id };
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