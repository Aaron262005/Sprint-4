using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sprint4.Backend.Application.Features.Users.Queries.GetUsers;

namespace Sprint4.Backend.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var query = new GetUsersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}