using ECX.Website.Application.CQRS.Image_.Request.Command;
using ECX.Website.Application.CQRS.Image_.Request.Queries;
using ECX.Website.Application.DTOs.Image;
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
    public class ImageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<ImageController>
        [HttpGet]
        public async Task<ActionResult<BaseCommonResponse>> Get()
        {
            var query = new GetImageListRequest();
            BaseCommonResponse response = await _mediator.Send(query);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;  
            }
        }

        // GET api/<ImageController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseCommonResponse>> Get(string id)
        {
            var query = new GetImageDetailRequest { Id = id };
            BaseCommonResponse response = await _mediator.Send(query);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;  
            }
        }

        // POST api/<ImageController>
        [HttpPost]
        public async Task<ActionResult<BaseCommonResponse>> Post([FromForm] ImageFormDto data)
        {
            var command = new CreateImageCommand { ImageFormDto = data };
            BaseCommonResponse response = await _mediator.Send(command);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;
                
            }

        }

        // PUT api/<ImageController>/5
        [HttpPut]
        public async Task<ActionResult<BaseCommonResponse>> Put([FromForm] ImageFormDto data)
        {
            var command = new UpdateImageCommand { ImageFormDto = data};
            BaseCommonResponse response = await _mediator.Send(command);
            switch(response.Status){
                case "200" : return Ok(response);
                case "400" : return BadRequest(response);
                case "404" : return NotFound(response);
                default : return response;
                
            }
        }

        // DELETE api/<ImageController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BaseCommonResponse>> Delete(string id)
        {
            var command = new DeleteImageCommand { Id = id };
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
