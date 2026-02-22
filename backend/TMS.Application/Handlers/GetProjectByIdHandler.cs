using MediatR;
using TMS.Application.DTOs.Responses;
using TMS.Application.Extensions;
using TMS.Application.Queries;
using TMS.Domain.Interfaces;

namespace TMS.Application.Handlers
{
    public sealed class GetProjectByIdHandler : IRequestHandler<GetProjectByIdQuery, ProjectResponse?>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectByIdHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectResponse?> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(query.Id);

            return project?.ToResponse();
        }
    }
}
