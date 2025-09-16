using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using TMS.Application.Commands;
using TMS.Application.DTOs.Requests;
using TMS.Application.DTOs.Responses;
using TMS.Application.Queries;

namespace TMS.Web.API.Controllers
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

        /// <summary>
        /// Gets a specific user by its ID
        /// </summary>
        /// <param name="id">Unique identifier of the task</param>
        /// <returns>The requested user or NotFound if it does not exists</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserResponse>> GetUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("User ID cannot be empty");
            }

            var query = new GetUserByIdQuery(id);

            var result = await _mediator.Send(query);

            if (result == null)
            {
                return this.NotFound($"User with ID {id} was not found");
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="request">User data to be created</param>
        /// <returns>The created user with 201 status or error responses</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
        {
            if (request == null)
            {
                return this.BadRequest("User request cannot be null");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return this.BadRequest("User name is required");
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return this.BadRequest("User email is required");
            }

            try
            {
                var command = new CreateUserCommand(request.Name, request.Email);

                var result = await _mediator.Send(command);

                return this.CreatedAtAction(
                    nameof(GetUser),
                    new { id = result.Id }, 
                    result
                );
            }
            catch(Exception ex)
            {
#if !DEBUG
                return this.StatusCode(500, "An error occurred while creating the user");
#else
                return this.StatusCode(500, ex.Message);
#endif
            }
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of all users or empty list if none exist</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetAllUsers()
        {
            var query = new GetAllUsersQuery();

            var result = await _mediator.Send(query);

            return this.Ok(result ?? new List<UserResponse>());
        }
    }
}
