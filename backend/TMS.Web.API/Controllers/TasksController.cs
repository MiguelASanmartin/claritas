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
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator) 
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets a specific task by its ID
        /// </summary>
        /// <param name="id">Unique identifier of the task</param>
        /// <returns>The requested task or NotFound if it does not exists</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TaskResponse>> GetTask(Guid id)
        {
            if (id == Guid.Empty) 
            {
                return this.BadRequest("Task ID cannot be empty");
            }

            var query = new GetTaskByIdQuery(id);

            var result = await _mediator.Send(query);

            if (result == null)
            {
                return this.NotFound($"Task with ID {id} was not found");
            }

            return this.Ok(result);
        }
        
        /// <summary>
        /// Inserts a new task
        /// </summary>
        /// <param name="request">The task to be inserted</param>
        /// <returns>The inserted task or InternalServerError if something goes wrong</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TaskResponse>> CreateTask([FromBody] CreateTaskRequest request)
        {
            if (request == null)
            {
                return this.BadRequest("Task cannot be null");
            }

            if (request.ProjectId == Guid.Empty)
            {
                return this.BadRequest("Task has to be related to a project");
            }

            if (request.AssignedToUserId == Guid.Empty)
            {
                return this.BadRequest("Task must be assigned to a user");
            }

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                return this.BadRequest("Task title is required");
            }

            try
            {
                var command = new CreateTaskCommand(request.Title, request.Description, request.ProjectId, request.AssignedToUserId, request.Priority, request.DueDate);

                var result = await _mediator.Send(command);

                return this.CreatedAtAction(nameof(GetTask), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
#if !DEBUG
                return this.StatusCode(500, "An error occurred while creating the task");
#else
                return this.StatusCode(500, ex.Message);
#endif
            }
        }

        /// <summary>
        /// Get all tasks from specified project
        /// </summary>
        /// <param name="projectId">Unique identifier of the project</param>
        /// <returns>The requested tasks from the specified project or empty list if none exist</returns>
        [HttpGet("by-project/{projectId}")]
        [ProducesResponseType(typeof(IEnumerable<TaskResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TaskResponse>>> GetTasksByProject(Guid projectId)
        {
            if (projectId == Guid.Empty) 
            {
                return this.BadRequest("Project ID cannot be empty");
            }

            var query = new GetTasksByProjectQuery(projectId);

            var result = await _mediator.Send(query);

            return this.Ok(result ?? new List<TaskResponse>());
        }

    }
}
