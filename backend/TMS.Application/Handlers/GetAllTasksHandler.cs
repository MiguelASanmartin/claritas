using MediatR;
using TMS.Application.DTOs.Responses;
using TMS.Application.Extensions;
using TMS.Application.Queries;
using TMS.Domain.Interfaces;

namespace TMS.Application.Handlers
{
    public sealed class GetAllTasksHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskResponse>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetAllTasksHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskResponse>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetAllAsync();

            return tasks.ToResponse();
        }
    }
}
