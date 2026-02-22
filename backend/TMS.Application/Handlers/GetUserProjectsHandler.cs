using MediatR;
using TMS.Application.DTOs.Responses;
using TMS.Application.Extensions;
using TMS.Application.Queries;
using TMS.Domain.Interfaces;

public sealed class GetUserProjectsHandler : IRequestHandler<GetUserProjectsQuery, IEnumerable<ProjectResponse>>
{
    private readonly IProjectRepository _projectRepository;

    public GetUserProjectsHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<IEnumerable<ProjectResponse>> Handle(GetUserProjectsQuery query, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetByUserIdAsync(query.UserId);
        return projects.ToResponse();
    }
}