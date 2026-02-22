using MediatR;
using TMS.Application.DTOs.Responses;
using TMS.Application.Extensions;
using TMS.Application.Queries;
using TMS.Domain.Interfaces;

namespace TMS.Application.Handlers
{
    public sealed class GetTaskByProjectHandler : IRequestHandler<GetTasksByProjectQuery, IEnumerable<TaskResponse>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskByProjectHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskResponse>> Handle(GetTasksByProjectQuery query, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetByProjectIdAsync(query.ProjectId);

            return tasks.ToResponse();
        }
    }
}
