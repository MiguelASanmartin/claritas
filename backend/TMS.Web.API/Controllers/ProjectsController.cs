using MediatR;
using Microsoft.AspNetCore.Mvc;
using TMS.Application.Commands;
using TMS.Application.DTOs.Requests;
using TMS.Application.DTOs.Responses;
using TMS.Application.Queries;

namespace TMS.Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a specified project by its ID
        /// </summary>
        /// <param name="id">Unique identifier of the project</param>
        /// <returns>The requested project or NotFound if it does not exists</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProjectResponse>> GetProject(Guid id)
        {
            if (id == Guid.Empty)
            {
                return this.BadRequest("Project ID cannot be empty");
            }

            var query = new GetProjectByIdQuery(id);

            var result = await _mediator.Send(query);

            if (result == null)
            {
                return this.NotFound($"Project with ID {id} was not found");
            }

            return this.Ok(result);
        }

        /// <summary>
        /// Gets all projects from specific user
        /// </summary>
        /// <param name="id">Unique identifier of the user</param>
        /// <returns>The requested projects from specified user or NotFound if it does not exists any</returns>
        [HttpGet("by-user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<ProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetProjectsByUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return this.BadRequest("User ID cannot be empty");
            }

            var query = new GetUserProjectsQuery(userId);

            var result = await _mediator.Send(query);

            return this.Ok(result ?? new List<ProjectResponse>());
        }

        /// <summary>
        /// Creates a new project
        /// </summary>
        /// <param name="request">Project data to be created</param>
        /// <returns>The created user with 201 status or error response</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ProjectResponse>> CreateProject([FromBody] CreateProjectRequest request)
        {
            if (request == null)
            {
                return this.BadRequest("Project request cannot be null");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return this.BadRequest("Project name is required");
            }

            if (request.OwnerId == Guid.Empty)
            {
                return this.BadRequest("Owner Id cannot be empty");
            }

            try
            {
                var command = new CreateProjectCommand(request.Name, request.Description, request.OwnerId, request.DueDate);

                var result = await _mediator.Send(command);

                return this.CreatedAtAction(
                    nameof(GetProject),
                    new { id = result.Id },
                    result
                );
            }
            catch (Exception ex)
            {
#if !DEBUG
                return this.StatusCode(500, "An error occurred while creating the project");
#else
                return this.StatusCode(500, ex.Message);
#endif
            }
        }
    }
}
