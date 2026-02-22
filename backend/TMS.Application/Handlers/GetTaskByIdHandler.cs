using MediatR;
using TMS.Application.DTOs.Responses;
using TMS.Application.Extensions;
using TMS.Application.Queries;
using TMS.Domain.Interfaces;

namespace TMS.Application.Handlers
{
    public sealed class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, TaskResponse?>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskByIdHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskResponse?> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(query.Id);

            return task?.ToResponse();
        }
    }
}
