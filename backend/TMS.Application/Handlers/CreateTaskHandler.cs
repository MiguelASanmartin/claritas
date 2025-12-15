using MediatR;
using TMS.Application.Commands;
using TMS.Application.DTOs.Responses;
using TMS.Application.Extensions;
using TMS.Domain.Interfaces;
using TMS.Domain.ValueObjects;

namespace TMS.Application.Handlers
{
    public sealed class CreateTaskHandler : IRequestHandler<CreateTaskCommand, TaskResponse>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserRepository _userRepository;

        public CreateTaskHandler(ITaskRepository taskRepository, IProjectRepository projectRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _userRepository = userRepository;
        }

        public async Task<TaskResponse> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
        {
            await ValidateProjectExists(command.ProjectId);
            await ValidateUserExists(command.AssignedToUserId);

            var priority = TaskPriority.FromString(command.Priority);

            var task = Domain.Entities.Task.Create(command.Title, command.ProjectId, command.AssignedToUserId);

            if (!string.IsNullOrWhiteSpace(command.Description))
                task.UpdateDescription(command.Description);

            task.ChangePriority(priority);

            await _taskRepository.AddAsync(task);

            var taskWithRelations = await _taskRepository.GetByIdAsync(task.Id)
                ?? throw new InvalidOperationException($"Failed to retrieve project with ID {task.Id} after creation");

            return taskWithRelations.ToResponse();
        }

        private async Task ValidateProjectExists(Guid projectId)
        {
            if (!await _projectRepository.ExistsAsync(projectId))
                throw new ArgumentException($"Project with ID {projectId} does not exists.");
        }

        private async Task ValidateUserExists(Guid userId)
        {
            if (!await _userRepository.ExistsAsync(userId))
                throw new ArgumentException($"User with ID {userId} does not exists.");
        }
    }
}
