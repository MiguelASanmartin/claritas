using MediatR;
using TMS.Application.Commands;
using TMS.Application.DTOs.Responses;
using TMS.Application.Extensions;
using TMS.Domain.Entities;
using TMS.Domain.Interfaces;

namespace TMS.Application.Handlers
{
    public sealed class CreateProjectHandler : IRequestHandler<CreateProjectCommand, ProjectResponse>
    {
        private readonly IProjectRepository _projectRepository;

        public CreateProjectHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectResponse> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
        {
            var project = Project.Create(command.Name, command.OwnerId, command.Description);

            if (command.DueDate.HasValue)
            {
                project.SetDueDate(command.DueDate);
            }

            await _projectRepository.AddAsync(project);

            return project.ToResponse();
        }
    }
}
